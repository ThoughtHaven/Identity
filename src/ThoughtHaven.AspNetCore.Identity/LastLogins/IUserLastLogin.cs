using System;

namespace ThoughtHaven.AspNetCore.Identity.LastLogins
{
    public interface IUserLastLogin
    {
        DateTimeOffset? LastLogin { get; set; }
    }
}