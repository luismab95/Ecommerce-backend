namespace Ecommerce.Infrastructure.Services;

using Ecommerce.Domain.DTOs.Email;
using Ecommerce.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;

public class GmailSmtpEmailSender(IConfiguration config, IHostEnvironment env) : IEmailService
{
    private readonly IConfiguration _config = config;
    private readonly IHostEnvironment _env = env;

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(message.From ?? _config["Gmail:Username"]));
        foreach (var to in message.To) email.To.Add(MailboxAddress.Parse(to));
        email.Subject = message.Subject ?? string.Empty;

        if (message.IsHtml)
            email.Body = new TextPart("html") { Text = message.Body };
        else
            email.Body = new TextPart("plain") { Text = message.Body };

        using var smtp = new SmtpClient();

        if (_env.IsDevelopment())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        }

        var host = _config["Gmail:SmtpHost"];
        var port = int.Parse(_config["Gmail:SmtpPort"]!);
        await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls, ct);

        await smtp.AuthenticateAsync(_config["Gmail:Username"], _config["Gmail:AppPassword"], ct);

        await smtp.SendAsync(email, ct);
        await smtp.DisconnectAsync(true, ct);
    }
}
