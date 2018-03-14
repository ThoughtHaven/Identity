using System;
using System.Security.Claims;
using static ThoughtHaven.Guard;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public class ClaimOptions
    {
        private string _issuer = ClaimsIdentity.DefaultIssuer;
        public string Issuer
        {
            get => this._issuer;
            set
            {
                this._issuer = NullOrWhiteSpace(nameof(value), value);
            }
        }

        private string _authenticationScheme = "ThoughtHavenIdentity";
        public virtual string AuthenticationScheme
        {
            get => this._authenticationScheme;
            set
            {
                this._authenticationScheme = NullOrWhiteSpace(nameof(value), value);
            }
        }

        public TimeSpan ValidateSecurityStampInterval { get; set; } = TimeSpan.FromMinutes(30);

        public virtual ClaimTypeOptions ClaimTypes { get; } = new ClaimTypeOptions();

        public class ClaimTypeOptions
        {
            private string _userKey = "sub";
            public string UserKey
            {
                get => this._userKey;
                set
                {
                    this._userKey = NullOrWhiteSpace(nameof(value), value);
                }
            }

            private string _securityStamp = "securitystamp";
            public string SecurityStamp
            {
                get => this._securityStamp;
                set
                {
                    this._securityStamp = NullOrWhiteSpace(nameof(value), value);
                }
            }

            private string _securityStampValidated = "securitystampvalidated";
            public string SecurityStampValidated
            {
                get => this._securityStampValidated;
                set
                {
                    this._securityStampValidated = NullOrWhiteSpace(nameof(value), value);
                }
            }
        }
    }
}