namespace ThoughtHaven.AspNetCore.Identity.Lockouts
{
    public class TimedLockout
    {
        public string Key { get; }
        public UtcDateTime LastModified { get; set; }
        public int FailedAccessAttempts { get; set; } = 1;
        public UtcDateTime? Expiration { get; set; } = null;

        public TimedLockout(string key, UtcDateTime lastModified)
        {
            this.Key = Guard.NullOrWhiteSpace(nameof(key), key);
            this.LastModified = lastModified;
        }
    }
}