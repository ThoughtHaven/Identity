using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakePasswordHasher : IPasswordHasher
    {
        public Password Hash_InputPassword;
        public string Hash_Output;
        public string Hash(Password password)
        {
            this.Hash_InputPassword = password;
            this.Hash_Output = "fakehash";

            return this.Hash_Output;
        }

        public string Validate_InputHash;
        public Password Validate_InputPassword;
        public PasswordValidateResult Validate_Output = new PasswordValidateResult(valid: true);
        public PasswordValidateResult Validate(string hash, Password password)
        {
            this.Validate_InputHash = hash;
            this.Validate_InputPassword = password;

            return Validate_Output;
        }
    }
}