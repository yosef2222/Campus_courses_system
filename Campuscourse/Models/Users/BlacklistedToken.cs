namespace Campuscourse.Models.Users;

public class BlacklistedToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}