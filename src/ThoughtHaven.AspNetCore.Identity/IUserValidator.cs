using System.Threading.Tasks;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity
{
    public interface IUserValidator<TUser> : IValidator<TUser, Task> { }
}