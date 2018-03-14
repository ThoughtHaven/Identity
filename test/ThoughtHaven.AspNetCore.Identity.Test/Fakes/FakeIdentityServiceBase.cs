using ThoughtHaven.AspNetCore.Identity.Internal;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeIdentityServiceBase : IdentityServiceBase<User>
    {
        public FakeUserStore UserStore { get; }
        public FakeUserValidators UserValidators { get; }
        public FakeClaimsAuthenticationService1 ClaimsAuthenticationService { get; }
        public FakeClaimsConverter ClaimsConverter { get; }

        public FakeIdentityServiceBase(FakeUserStore userStore,
            FakeUserValidators userValidators,
            FakeClaimsAuthenticationService1 claimsAuthenticationService,
            FakeClaimsConverter claimsConverter)
            : base(userStore, userValidators, claimsAuthenticationService, claimsConverter)
        {
            this.UserStore = userStore;
            this.UserValidators = userValidators;
            this.ClaimsAuthenticationService = claimsAuthenticationService;
            this.ClaimsConverter = claimsConverter;
        }
    }
}