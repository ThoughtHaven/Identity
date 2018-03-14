using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeClaimsAuthenticationService1 : IAuthenticationService<ClaimsPrincipal>
    {
        public bool Login_Called = false;
        public ClaimsPrincipal Login_InputPrincipal;
        public AuthenticationProperties Login_InputProperties;
        public Task Login(ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            this.Login_Called = true;
            this.Login_InputPrincipal = principal;
            this.Login_InputProperties = properties;

            return Task.CompletedTask;
        }

        public bool Authenticate_Called = false;
        public ClaimsPrincipal Authenticate_OutputPrincipal = new ClaimsPrincipal();
        public Task<ClaimsPrincipal> Authenticate()
        {
            this.Authenticate_Called = true;

            return Task.FromResult(this.Authenticate_OutputPrincipal);
        }

        public bool Logout_Called = false;
        public Task Logout()
        {
            this.Logout_Called = true;

            return Task.CompletedTask;
        }
    }
}