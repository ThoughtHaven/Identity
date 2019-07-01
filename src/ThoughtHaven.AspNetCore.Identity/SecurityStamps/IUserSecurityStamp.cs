namespace ThoughtHaven.AspNetCore.Identity.SecurityStamps
{
    public interface IUserSecurityStamp
    {
        string? SecurityStamp { get; set; }
    }
}