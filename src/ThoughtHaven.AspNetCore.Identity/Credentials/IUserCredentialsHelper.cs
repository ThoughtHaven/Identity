namespace ThoughtHaven.AspNetCore.Identity
{
    public partial interface IUserHelper
    {
        UserMessage InvalidCredentials { get; }
    }
}