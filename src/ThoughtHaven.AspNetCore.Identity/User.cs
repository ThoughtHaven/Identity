using System;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class User
        : IUserKey, IUserId, IUserEmail, IUserPasswordHash, IUserSecurityStamp, IUserCreated,
        IUserLastLogin
    {
        public virtual UserKey Key() =>
            string.IsNullOrWhiteSpace(this.Id) ? default(UserKey) : new UserKey(this.Id);

        public virtual string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLogin { get; set; }
    }
}