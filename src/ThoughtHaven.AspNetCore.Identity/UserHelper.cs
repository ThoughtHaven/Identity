namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper : IUserHelper
    {
        protected abstract SystemClock Clock { get; }
    }
}