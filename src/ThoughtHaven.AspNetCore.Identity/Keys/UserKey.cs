namespace ThoughtHaven.AspNetCore.Identity.Keys
{
    public class UserKey : ValueObject<string>
    {
        public UserKey(string value) : base(value)
        {
            Guard.NullOrWhiteSpace(nameof(value), value);
        }

        public static implicit operator UserKey(string value) => new UserKey(value);
    }
}