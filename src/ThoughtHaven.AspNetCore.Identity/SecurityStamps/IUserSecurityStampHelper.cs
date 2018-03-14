using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.SecurityStamps;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        Task RefreshSecurityStamp<TUser>(TUser user) where TUser : IUserSecurityStamp;
    }
}