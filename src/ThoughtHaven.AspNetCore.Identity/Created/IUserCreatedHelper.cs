using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Created;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        Task SetCreated<TUser>(TUser user) where TUser : IUserCreated;
    }
}