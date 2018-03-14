namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public interface IUserPasswordHash
    {
        string PasswordHash { get; set; }
    }
}