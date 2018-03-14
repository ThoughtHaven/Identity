using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Emails
{
    public class UserEmailRequiredValidator<TUser> : IUserValidator<TUser>
        where TUser : IUserEmail
    {
        public virtual Task Validate(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument must have an {nameof(user.Email)}.");
            }

            return Task.CompletedTask;
        }
    }
}