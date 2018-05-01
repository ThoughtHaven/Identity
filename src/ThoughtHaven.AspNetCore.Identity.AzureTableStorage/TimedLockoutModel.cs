using System;
using ThoughtHaven.AspNetCore.Identity.Lockouts;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TimedLockoutModel
    {
        public string Key { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public int FailedAccessAttempts { get; set; }
        public DateTimeOffset? Expiration { get; set; }

        public TimedLockoutModel(TimedLockout lockout) : this()
        {
            Guard.Null(nameof(lockout), lockout);

            this.Key = lockout.Key;
            this.LastModified = lockout.LastModified;
            this.FailedAccessAttempts = lockout.FailedAccessAttempts;
            this.Expiration = lockout.Expiration;
        }

        public TimedLockoutModel() { }
    }
}