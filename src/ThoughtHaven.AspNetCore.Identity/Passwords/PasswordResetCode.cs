using ThoughtHaven.AspNetCore.Identity.Internal;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class PasswordResetCode : VerificationCode
    {
        public PasswordResetCode(int value) : base(value) { }

        public static implicit operator PasswordResetCode(int value) =>
            new PasswordResetCode(value);
    }
}