using System;
using System.Threading.Tasks;
using ThoughtHaven.Security.SingleUseTokens;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeSingleUseTokenService : ISingleUseTokenService
    {
        public bool Create_Called => this.Create_InputToken != null;
        public SingleUseToken Create_InputToken;
        public DateTimeOffset Create_InputExpiration;
        public Task Create(SingleUseToken token, DateTimeOffset expiration)
        {
            this.Create_InputToken = token;
            this.Create_InputExpiration = expiration;

            return Task.CompletedTask;
        }
        
        public SingleUseToken Validate_InputToken;
        public bool Validate_Output = true;
        public Task<bool> Validate(SingleUseToken token)
        {
            this.Validate_InputToken = token;

            return Task.FromResult(this.Validate_Output);
        }
    }
}