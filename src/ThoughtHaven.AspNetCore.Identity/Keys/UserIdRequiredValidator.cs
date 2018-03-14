using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Keys
{
    public class UserIdRequiredValidator<TUser> : IUserValidator<TUser> where TUser : IUserId
    {
        public virtual Task Validate(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument must have an {nameof(user.Id)}.");
            }

            return Task.CompletedTask;
        }
    }
}