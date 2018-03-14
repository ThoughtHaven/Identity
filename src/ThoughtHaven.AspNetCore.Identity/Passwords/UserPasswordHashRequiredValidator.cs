using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class UserPasswordHashRequiredValidator<TUser> : IUserValidator<TUser>
        where TUser : IUserPasswordHash
    {
        public virtual Task Validate(TUser user)
        {
            Guard.Null(nameof(user), user);

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException(
                    paramName: nameof(user),
                    message: $"The {nameof(user)} argument must have a {nameof(user.PasswordHash)}.");
            }

            return Task.CompletedTask;
        }
    }
}