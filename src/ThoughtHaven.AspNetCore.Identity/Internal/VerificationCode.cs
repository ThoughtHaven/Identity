using System;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public class VerificationCode : ValueObject<int>
    {
        public VerificationCode(int value)
            : base(value)
        {
            Guard.LessThan(nameof(value), value, minimum: 0);
            
            if (value.ToString().Length < 4)
            {
                throw new ArgumentException(
                    paramName: nameof(value),
                    message: $"For security reasons, the {nameof(value)} argument must be at least 4 characters long. Better yet 6 or 8.");
            }
        }

        public static implicit operator VerificationCode(int value) =>
            new VerificationCode(value);
    }
}