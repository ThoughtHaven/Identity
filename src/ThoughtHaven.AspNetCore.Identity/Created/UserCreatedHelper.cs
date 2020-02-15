using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Created;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual Task SetCreated<TUser>(TUser user) where TUser : IUserCreated
        {
            Guard.Null(nameof(user), user);

            user.Created = this.Clock.UtcNow.ToOffset();

            return Task.CompletedTask;
        }
    }
}