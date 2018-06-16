using System.Threading;
using System.Threading.Tasks;
using PwnedPasswords.Client;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakePwnedPasswordClient : IPwnedPasswordsClient
    {
        public string HasPasswordBeenPwned_InputPassword;
        public bool HasPasswordBeenPwned_Output;
        public Task<bool> HasPasswordBeenPwned(string password,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            this.HasPasswordBeenPwned_InputPassword = password;

            return Task.FromResult(this.HasPasswordBeenPwned_Output);
        }
    }
}