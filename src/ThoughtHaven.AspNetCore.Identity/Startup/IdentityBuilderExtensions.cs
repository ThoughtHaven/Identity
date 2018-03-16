using Microsoft.Extensions.DependencyInjection;
using System;
using ThoughtHaven;

namespace Microsoft.AspNetCore.Builder
{
    public static class IdentityBuilderExtensions
    {
        public static IApplicationBuilder UseThoughtHavenIdentity(this IApplicationBuilder app)
        {
            Guard.Null(nameof(app), app);

            var options = app.ApplicationServices.GetService<IdentityOptions>();
            if (options == null)
            {
                throw new InvalidOperationException($"Unable to find {nameof(IdentityOptions)} registered in the application services. Please add all the required services by calling '{nameof(IServiceCollection)}.{nameof(IdentityServiceCollectionExtensions.AddThoughtHavenIdentity)}' inside the call to 'ConfigureServices(...)' in the application startup code.");
            }

            return app.UseAuthentication();
        }
    }
}