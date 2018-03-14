using System;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual Task AssignUserId<TUser>(TUser user) where TUser : IUserId
        {
            Guard.Null(nameof(user), user);

            user.Id = Guid.NewGuid().ToString("N");

            return Task.CompletedTask;
        }
    }
}