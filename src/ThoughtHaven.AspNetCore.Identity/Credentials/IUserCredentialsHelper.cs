namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UiMessage InvalidCredentials { get; }
    }
}