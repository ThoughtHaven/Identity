using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class MinimumLengthPasswordStrengthValidator : IPasswordStrengthValidator
    {
        protected virtual byte MinimumLength => 8;
        protected virtual UserMessage InvalidPasswordStrength { get; }

        public MinimumLengthPasswordStrengthValidator()
        {
            this.InvalidPasswordStrength = new UserMessage($"A password needs to be at least {this.MinimumLength} characters long.");
        }

        public virtual Task<Result<UserMessage>> Validate(Password password)
        {
            Guard.Null(nameof(password), password);

            if (password.Value.Length < this.MinimumLength)
            { return Task.FromResult<Result<UserMessage>>(this.InvalidPasswordStrength); }

            return Task.FromResult(new Result<UserMessage>());
        }
    }
}