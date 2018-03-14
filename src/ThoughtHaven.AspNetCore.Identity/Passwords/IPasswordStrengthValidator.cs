using System.Threading.Tasks;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Passwords
{
    public interface IPasswordStrengthValidator
        : IValidator<Password, Task<Result<UserMessage>>>
    { }
}