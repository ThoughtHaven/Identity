using System;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public struct PasswordValidateResult
    {
        public bool Valid { get; }
        public bool UpdateHash { get; }

        public PasswordValidateResult(bool valid, bool updateHash = false)
        {
            if (!valid && updateHash)
            {
                throw new ArgumentException(paramName: nameof(updateHash),
                    message: $"The {nameof(updateHash)} argument can only be true when the {nameof(valid)} argument is also true.");
            }

            this.Valid = valid;
            this.UpdateHash = updateHash;
        }
    }
}