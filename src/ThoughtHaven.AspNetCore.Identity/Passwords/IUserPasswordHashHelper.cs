using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UserMessage InvalidPassword { get; }
        UserMessage InvalidPasswordResetCode { get; }
        Task<Result<UserMessage>> SetPasswordHash<TUser>(TUser user, Password password)
            where TUser : IUserPasswordHash;
        Task<PasswordValidateResult> ValidatePassword<TUser>(TUser user, Password password)
            where TUser : IUserPasswordHash;
        Task<PasswordResetCode> CreatePasswordResetCode(UserKey userKey);
        Task<bool> ValidatePasswordResetCode(UserKey userKey, PasswordResetCode code);
    }
}