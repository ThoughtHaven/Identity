using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeMinimumLengthPasswordStrengthValidator
        : MinimumLengthPasswordStrengthValidator
    {
        new public byte MinimumLength => base.MinimumLength;
        new public UiMessage InvalidPasswordStrength => base.InvalidPasswordStrength;
    }
}