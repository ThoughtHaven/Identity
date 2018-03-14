using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.SecurityStamps
{
    public class UserSecurityStampRequiredValidator<TUser> : IUserValidator<TUser>
        where TUser : IUserSecurityStamp
    {
        public virtual Task Validate(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.SecurityStamp))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument must have a {nameof(user.SecurityStamp)}.");
            }

            return Task.CompletedTask;
        }
    }
}