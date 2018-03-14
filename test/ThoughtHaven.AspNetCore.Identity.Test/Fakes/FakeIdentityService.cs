using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeIdentityService : IdentityService<User>
    {
        public FakeUserStore UserStore { get; }
        public FakeUserValidators UserValidators { get; }
        public FakeClaimsAuthenticationService1 ClaimsAuthenticationService { get; }
        public FakeClaimsConverter ClaimsConverter { get; }
        new public FakeUserHelper1 Helper => (FakeUserHelper1)base.Helper;

        public FakeIdentityService(FakeUserStore userStore,
            FakeUserValidators userValidators,
            FakeClaimsAuthenticationService1 claimsAuthenticationService,
            FakeClaimsConverter claimsConverter, FakeUserHelper1 helper)
            : base(userStore, userValidators, claimsAuthenticationService, claimsConverter,
                  helper)
        {
            this.UserStore = userStore;
            this.UserValidators = userValidators;
            this.ClaimsAuthenticationService = claimsAuthenticationService;
            this.ClaimsConverter = claimsConverter;
        }

        public UserKey Retrieve_InputKey;
        public override Task<User> Retrieve(UserKey key)
        {
            this.Retrieve_InputKey = key;

            return base.Retrieve(key);
        }

        public User Create_InputUser;
        public override Task<User> Create(User user)
        {
            this.Create_InputUser = user;

            return base.Create(user);
        }

        public User Update_InputUser;
        public override Task<User> Update(User user)
        {
            this.Update_InputUser = user;

            return base.Update(user);
        }

        public User Login_InputUser;
        public AuthenticationProperties Login_InputProperties;
        public override Task Login(User user, AuthenticationProperties properties)
        {
            this.Login_InputUser = user;
            this.Login_InputProperties = properties;

            return base.Login(user, properties);
        }

        public bool Authenticate_Called;
        public User Authenticate_OutputOverride;
        public override Task<User> Authenticate()
        {
            this.Authenticate_Called = true;

            return this.Authenticate_OutputOverride == null ? base.Authenticate()
                : Task.FromResult(this.Authenticate_OutputOverride);
        }

        public bool Logout_Called;
        public override Task Logout()
        {
            this.Logout_Called = true;

            return base.Logout();
        }
    }
}