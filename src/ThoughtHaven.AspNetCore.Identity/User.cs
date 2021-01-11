using System;
using ThoughtHaven.AspNetCore.Identity.Created;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.LastLogins;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public record User
        : IUserKey, IUserId, IUserEmail, IUserPasswordHash, IUserSecurityStamp, IUserCreated,
        IUserLastLogin
    {
        public virtual UserKey? Key() =>
            string.IsNullOrWhiteSpace(this.Id) ? default : new UserKey(this.Id!);

        public virtual string? Id { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public virtual string? SecurityStamp { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastLogin { get; set; }

        public User() { }

        public User(User user)
        {
            Guard.Null(nameof(user), user);

            this.Id = user.Id;
            this.Email = user.Email;
            this.EmailConfirmed = user.EmailConfirmed;
            this.PasswordHash = user.PasswordHash;
            this.SecurityStamp = user.SecurityStamp;
            this.Created = user.Created;
            this.LastLogin = user.LastLogin;
        }
    }
}