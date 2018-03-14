using System;
using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserValidator : IUserValidator<User>
    {
        private readonly bool _invalid;

        public FakeUserValidator(bool invalid = false)
        {
            this._invalid = invalid;
        }

        public bool Validate_Called = false;
        public User Validate_UserInput;
        public Task Validate(User user)
        {
            this.Validate_Called = true;
            this.Validate_UserInput = user;

            if (this._invalid) { throw new Exception(); }

            return Task.CompletedTask;
        }
    }
}