using System;

namespace ThoughtHaven.AspNetCore.Identity.Lockouts
{
    public class TimedLockout
    {
        public string Key { get; }
        public DateTimeOffset LastModified { get; set; }
        public int FailedAccessAttempts { get; set; } = 1;
        public DateTimeOffset? Expiration { get; set; } = null;

        public TimedLockout(string key, DateTimeOffset lastModified)
        {
            this.Key = Guard.NullOrWhiteSpace(nameof(key), key);
            this.LastModified = lastModified;
        }
    }
}