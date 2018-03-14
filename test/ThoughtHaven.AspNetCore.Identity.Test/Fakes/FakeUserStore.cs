using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Data;
using ThoughtHaven.Messages.Emails;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserStore
        : ICrudStore<UserKey, User>, IRetrieveOperation<EmailAddress, User>
    {
        public UserKey Retrieve_ByKey_InputUserKey;
        public User Retrieve_ByKey_Output = new User()
        {
            Id = "id",
            SecurityStamp = "stamp"
        };
        public Task<User> Retrieve(UserKey key)
        {
            this.Retrieve_ByKey_InputUserKey = key;

            return Task.FromResult(this.Retrieve_ByKey_Output);
        }

        public User Create_InputUser;
        public User Create_Output;
        public Task<User> Create(User create)
        {
            this.Create_InputUser = create;
            this.Create_Output = create;

            return Task.FromResult(this.Create_Output);
        }

        public User Update_InputUser;
        public User Update_Output;
        public Task<User> Update(User update)
        {
            this.Update_InputUser = update;
            this.Update_Output = update;

            return Task.FromResult(this.Update_Output);
        }

        public UserKey Deleted_InputUserKey;
        public Task Delete(UserKey key)
        {
            this.Deleted_InputUserKey = key;

            return Task.FromResult(0);
        }
        
        public bool Retrieve_ByEmail_Called = false;
        public EmailAddress Retrieve_ByEmail_InputEmail;
        public User Retrieve_ByEmail_Output = new User()
        {
            Id = "id",
            SecurityStamp = "stamp",
            Email = "some@email.com",
            PasswordHash = "fakehash",
        };
        public Task<User> Retrieve(EmailAddress email)
        {
            this.Retrieve_ByEmail_Called = true;
            this.Retrieve_ByEmail_InputEmail = email;

            return Task.FromResult(Retrieve_ByEmail_Output);
        }
    }
}