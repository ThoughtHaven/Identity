using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeTimedLockoutStore : ICrudStore<string, TimedLockout>
    {
        public string? Retrieve_KeyInput;
        public TimedLockout? Retrieve_Output = new TimedLockout("key", DateTimeOffset.UtcNow);
        public Task<TimedLockout?> Retrieve(string key)
        {
            this.Retrieve_KeyInput = key;
            
            return Task.FromResult(this.Retrieve_Output);
        }

        public TimedLockout? Create_InputData;
        public Task<TimedLockout> Create(TimedLockout create)
        {
            this.Create_InputData = create;

            return Task.FromResult(create);
        }

        public TimedLockout? Update_InputData;
        public Task<TimedLockout> Update(TimedLockout update)
        {
            this.Update_InputData = update;

            return Task.FromResult(update);
        }

        public string? Delete_KeyInput;
        public Task Delete(string key)
        {
            this.Delete_KeyInput = key;

            return Task.CompletedTask;
        }
    }
}