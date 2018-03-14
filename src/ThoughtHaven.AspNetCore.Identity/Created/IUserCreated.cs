using System;

namespace ThoughtHaven.AspNetCore.Identity.Created
{
    public interface IUserCreated
    {
        DateTimeOffset Created { get; set; }
    }
}