namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public class Password : ValueObject<string>
    {
        public Password(string value) : base(value)
        {
            Guard.NullOrWhiteSpace(nameof(value), value);
        }

        public static implicit operator Password(string value) => new Password(value);
    }
}