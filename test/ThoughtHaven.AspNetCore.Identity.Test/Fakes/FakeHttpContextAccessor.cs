using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Claims;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public AuthenticationService Authentication { get; }
        public HttpContext HttpContext { get; set; }

        public FakeHttpContextAccessor()
        {
            var authentication = new AuthenticationService();

            var services = new ServiceCollection();
            services.AddSingleton<IAuthenticationService>(authentication);

            var context = new DefaultHttpContext()
            {
                RequestServices = services.BuildServiceProvider()
            };

            this.Authentication = authentication;
            this.HttpContext = context;
        }

        public class AuthenticationService : IAuthenticationService
        {
            public HttpContext? SignInAsync_InputContext;
            public string? SignInAsync_InputScheme;
            public ClaimsPrincipal? SignInAsync_InputPrincipal;
            public AuthenticationProperties? SignInAsync_InputProperties;
            public Task SignInAsync(HttpContext context, string scheme,
                ClaimsPrincipal principal, AuthenticationProperties properties)
            {
                this.SignInAsync_InputContext = context;
                this.SignInAsync_InputScheme = scheme;
                this.SignInAsync_InputPrincipal = principal;
                this.SignInAsync_InputProperties = properties;

                return Task.CompletedTask;
            }

            public HttpContext? AuthenticateAsync_InputContext;
            public string? AuthenticateAsync_InputScheme;
            public AuthenticateResult? AuthenticateAsync_Output = AuthenticateResult.Success(
                new AuthenticationTicket(FakeClaimsAuthenticationServiceBase.Principal(),
                    new AuthenticationProperties(), new ClaimOptions().AuthenticationScheme));
            public Task<AuthenticateResult?> AuthenticateAsync(HttpContext context,
                string scheme)
            {
                this.AuthenticateAsync_InputContext = context;
                this.AuthenticateAsync_InputScheme = scheme;

                return Task.FromResult(this.AuthenticateAsync_Output);
            }

            public bool SignOutAsync_Called => this.SignOutAsync_InputContext != null;
            public HttpContext? SignOutAsync_InputContext;
            public string? SignOutAsync_InputScheme;
            public AuthenticationProperties? SignOutAsync_InputProperties;
            public Task SignOutAsync(HttpContext context, string scheme,
                AuthenticationProperties properties)
            {
                this.SignOutAsync_InputContext = context;
                this.SignOutAsync_InputScheme = scheme;
                this.SignOutAsync_InputProperties = properties;

                return Task.CompletedTask;
            }

            public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties) => throw new NotImplementedException();
            public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties) => throw new NotImplementedException();
        }
    }
}