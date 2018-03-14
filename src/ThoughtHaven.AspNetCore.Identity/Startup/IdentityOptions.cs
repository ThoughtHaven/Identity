using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public class IdentityOptions
    {
        private string _authenticationScheme;
        public virtual string AuthenticationScheme
        {
            get { return _authenticationScheme; }
            set
            {
                _authenticationScheme = Guard.Null(nameof(value), value);

                this.Claims.AuthenticationScheme = _authenticationScheme;
            }
        }

        private ClaimOptions _claims = new ClaimOptions();
        public virtual ClaimOptions Claims
        {
            get { return _claims; }
            set { _claims = Guard.Null(nameof(value), value); }
        }

        private UserOptions _user = new UserOptions();
        public virtual UserOptions User
        {
            get { return _user; }
            set { _user = Guard.Null(nameof(value), value); }
        }

        private CookieAuthenticationOptions _cookie = new CookieAuthenticationOptions()
        {
            Cookie = new CookieBuilder()
            {
                Name = ".identity",
                HttpOnly = true,
                SecurePolicy = CookieSecurePolicy.Always
            },
            ExpireTimeSpan = TimeSpan.FromDays(7),
            SlidingExpiration = true,
        };
        public virtual CookieAuthenticationOptions Cookie
        {
            get { return _cookie; }
            set { _cookie = Guard.Null(nameof(value), value); }
        }

        public IdentityOptions()
        {
            this._authenticationScheme = this.Claims.AuthenticationScheme;
        }
    }
}