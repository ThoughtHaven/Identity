using ThoughtHaven.AspNetCore.Identity.Internal;

namespace ThoughtHaven.AspNetCore.Identity
{
    public interface IIdentityService<TUser> : IIdentityServiceBase<TUser>
    {
        IUserHelper Helper { get; }
    }
}