using System;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeSystemClock : SystemClock
    {
        public override DateTimeOffset UtcNow { get; }

        public FakeSystemClock() : this(DateTimeOffset.UtcNow) { }

        public FakeSystemClock(DateTimeOffset utcNow)
        {
            this.UtcNow = utcNow;
        }
    }
}