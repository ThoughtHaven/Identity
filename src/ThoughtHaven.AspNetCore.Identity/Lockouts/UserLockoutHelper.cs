using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        protected abstract ICrudStore<string, TimedLockout> TimedLockoutStore { get; }

        public virtual UiMessage LockedOut { get; }
            = new UiMessage("This account has been locked to protect it from possible hacking. Wait a few minutes to try again.");

        public virtual Task<bool> IsLockedOut(string key) =>
            this.IsLockedOut(key, TimeSpan.FromMinutes(10), maxFailedAccessAttempts: 5);

        protected virtual async Task<bool> IsLockedOut(string key, TimeSpan lockoutLength,
            byte maxFailedAccessAttempts)
        {
            Guard.NullOrWhiteSpace(nameof(key), key);
            Guard.LessThan(nameof(maxFailedAccessAttempts), maxFailedAccessAttempts,
                minimum: (byte)2);

            var now = this.Clock.UtcNow;

            var lockout = await this.TimedLockoutStore.Retrieve(key).ConfigureAwait(false);

            if (lockout == null)
            {
                lockout = new TimedLockout(key, lastModified: now);

                await this.TimedLockoutStore.Create(lockout).ConfigureAwait(false);
            }
            else if (lockout.Expiration is null)
            {
                if (lockout.LastModified.ToOffset() <= now.ToOffset().Subtract(lockoutLength))
                {
                    lockout = new TimedLockout(key, lastModified: now);

                    await this.TimedLockoutStore.Update(lockout).ConfigureAwait(false);
                }
                else
                {
                    lockout.LastModified = now;
                    lockout.FailedAccessAttempts++;

                    if (lockout.FailedAccessAttempts >= maxFailedAccessAttempts)
                    {
                        lockout.Expiration = new UtcDateTime(
                            now.ToOffset().Add(lockoutLength));
                    }

                    await this.TimedLockoutStore.Update(lockout).ConfigureAwait(false);
                }
            }
            else if (lockout.Expiration <= now)
            {
                lockout = new TimedLockout(key, lastModified: now);

                await this.TimedLockoutStore.Update(lockout).ConfigureAwait(false);
            }

            return !(lockout.Expiration is null) && lockout.Expiration > now;
        }

        public virtual Task ResetLockedOut(string key) =>
            this.TimedLockoutStore.Delete(Guard.NullOrWhiteSpace(nameof(key), key));
    }
}