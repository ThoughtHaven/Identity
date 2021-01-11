using PwnedPasswords.Client;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PwnedPasswordStrengthValidator : IPasswordStrengthValidator
    {
        private readonly IPwnedPasswordsClient _pwned;

        public PwnedPasswordStrengthValidator(IPwnedPasswordsClient pwned)
        {
            this._pwned = Guard.Null(nameof(pwned), pwned);
        }

        public async Task<Result<UiMessage>> Validate(Password password)
        {
            Guard.Null(nameof(password), password);

            var insecure = await this._pwned.HasPasswordBeenPwned(password.Value)
                .ConfigureAwait(false);

            return !insecure ? new Result<UiMessage>()
                : new UiMessage("This password is insecure because it was exposed in an online data breach. You'll need a stronger password to protect your account. You can learn more at https://haveibeenpwned.com.");
        }
    }
}