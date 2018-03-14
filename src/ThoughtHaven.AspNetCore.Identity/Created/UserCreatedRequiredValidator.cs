using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Created
{
    public class UserCreatedRequiredValidator<TUser> : IUserValidator<TUser>
        where TUser : IUserCreated
    {
        public virtual Task Validate(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (user.Created == default(DateTimeOffset))
            {
                throw new ArgumentException(paramName: nameof(user),
                    message: $"The {nameof(user)} argument must have its {nameof(user.Created)} property set.");
            }

            return Task.CompletedTask;
        }
    }
}