namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public interface IPasswordHasher
    {
        string Hash(Password password);
        PasswordValidateResult Validate(string hash, Password password);
    }
}