using ThoughtHaven.AspNetCore.Identity.Internal;

namespace ThoughtHaven.AspNetCore.Identity
{
    public interface IIdentityService<TUser> : IIdentityServiceBase<TUser>
        where TUser : class
    {
        IUserHelper Helper { get; }
    }
}