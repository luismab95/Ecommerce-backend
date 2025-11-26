namespace Ecommerce.Domain.Entities;

public class Session
{
    public int Id { get; private set; }
    public int UserId { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime LoginAt { get; set; }
    public DateTime? LogoutAt { get; set; }
    public bool IsActive { get; set; } = true;

   
    public virtual User? User { get; set; }

    private Session() { }

    public static Session Create(int userId, string deviceInfo, string ipAddress, string refreshToken, bool isActive)
    {
        return new Session
        {
            UserId = userId,
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            RefreshToken = refreshToken,
            IsActive = isActive
        };
    }
}