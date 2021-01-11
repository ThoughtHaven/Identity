using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.AspNetCore.Identity.Internal;
using ThoughtHaven.Contacts;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UiMessage UserAlreadyOwnsEmail { get; }
        UiMessage EmailNotAvailable { get; }
        UiMessage InvalidEmailVerificationCode { get; }
        Task<TUser?> Retrieve<TUser>(EmailAddress email) where TUser : class, IUserEmail;
        Task SetEmail<TUser>(TUser user, EmailAddress email) where TUser : IUserEmail;
        Task ConfirmEmail<TUser>(TUser user) where TUser : IUserEmail;
        Task<VerificationCode> CreateEmailVerificationCode(UserKey userKey);
        Task<bool> ValidateEmailVerificationCode(UserKey userKey,
            VerificationCode code);
    }
}