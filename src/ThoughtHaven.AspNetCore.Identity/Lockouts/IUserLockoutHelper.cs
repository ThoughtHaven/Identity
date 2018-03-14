using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UserMessage LockedOut { get; }
        Task<bool> IsLockedOut(string key);
    }
}