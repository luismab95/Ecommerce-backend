namespace Ecommerce.Domain.Entities;

public class Session
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string DeviceInfo { get; private set; } = string.Empty;
    public string IpAddress { get; private set; } = string.Empty;
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTime LoginAt { get; private set; }
    public DateTime? LogoutAt { get; private set; }
    public bool IsActive { get; private set; } = true;


    public virtual User? User { get; set; }

    private Session() { }


}