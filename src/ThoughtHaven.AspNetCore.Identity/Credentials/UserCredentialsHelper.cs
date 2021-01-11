namespace ThoughtHaven.AspNetCore.Identity
{
    public abstract partial class UserHelper
    {
        public virtual UiMessage InvalidCredentials { get; }
            = "Those credentials weren't right. Go ahead and try again.";
    }
}