using System.Threading.Tasks;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserEmailStore : IRetrieveOperation<EmailAddress, User>
    {
        public bool Retrieve_Called = false;
        public EmailAddress Retrieve_InputEmail;
        public User Retrieve_Output = new User()
        {
            Id = "1",
            SecurityStamp = "Stamp",
            PasswordHash = "fakehash",
        };
        public Task<User> Retrieve(EmailAddress email)
        {
            this.Retrieve_Called = true;
            this.Retrieve_InputEmail = email;

            return Task.FromResult(Retrieve_Output);
        }
    }
}