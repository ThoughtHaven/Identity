using ThoughtHaven.AspNetCore.Identity.Passwords;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakePbkdf2PasswordHasher : Pbkdf2PasswordHasher
    {
        new public int Iterations => base.Iterations;

        public FakePbkdf2PasswordHasher() : base() { }

        public FakePbkdf2PasswordHasher(int iterations) : base(iterations) { }
    }
}