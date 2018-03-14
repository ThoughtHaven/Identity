namespace ThoughtHaven.AspNetCore.Identity.Emails
{
    public interface IUserEmail
    {
        string Email { get; set; }
        bool EmailConfirmed { get; set; }
    }
}