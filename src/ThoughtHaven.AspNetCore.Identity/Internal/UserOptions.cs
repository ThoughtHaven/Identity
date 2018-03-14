namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class UserOptions
    {
        public bool RequireId { get; set; } = true;
        public bool RequireCreated { get; set; } = true;
        public bool RequireSecurityStamp { get; set; } = true;
        public bool RequireEmail { get; set; } = true;
        public bool RequirePassword { get; set; } = true;
    }
}