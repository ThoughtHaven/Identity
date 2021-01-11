using System.Threading.Tasks;

namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UiMessage LockedOut { get; }
        Task<bool> IsLockedOut(string key);
        Task ResetLockedOut(string key);
    }
}