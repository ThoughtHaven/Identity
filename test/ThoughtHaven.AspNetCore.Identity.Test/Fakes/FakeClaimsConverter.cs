using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Claims;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeClaimsConverter : IUserClaimsConverter<User>
    {
        public User? Convert_FromUser_InputUser;
        public ClaimsPrincipal Convert_FromUser_Output = new ClaimsPrincipal();
        public Task<ClaimsPrincipal> Convert(User user)
        {
            this.Convert_FromUser_InputUser = user;

            return Task.FromResult(this.Convert_FromUser_Output);
        }

        public ClaimsPrincipal? Convert_FromPrincipal_InputPrincipal;
        public UserKey? Convert_FromPrincipal_Output = "key";
        public Task<UserKey?> Convert(ClaimsPrincipal principal)
        {
            this.Convert_FromPrincipal_InputPrincipal = principal;

            return Task.FromResult(this.Convert_FromPrincipal_Output);
        }
    }
}