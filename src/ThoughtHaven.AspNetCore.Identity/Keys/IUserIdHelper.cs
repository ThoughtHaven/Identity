using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        Task AssignUserId<TUser>(TUser user) where TUser : IUserId;
    }
}