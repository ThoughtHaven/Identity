using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.LastLogins;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        Task SetLastLogin<TUser>(TUser user) where TUser : IUserLastLogin;
    }
}