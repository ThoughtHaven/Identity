using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual Task RefreshSecurityStamp<TUser>(TUser user)
            where TUser : IUserSecurityStamp
        {
            Guard.Null(nameof(user), user);

            user.SecurityStamp = this._random.GenerateString(length: 20);

            return Task.CompletedTask;
        }
    }
}