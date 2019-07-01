using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity.Startup
{
    public class IdentityBuilderExtensionsTests
    {
        public class UseThoughtHavenIdentityMethod
        {
            public class AppOverload
            {
                [Fact]
                public void NullApp_Throws()
                {
                    IApplicationBuilder? app = null;

                    Assert.Throws<ArgumentNullException>("app", () =>
                    {
                        app!.UseThoughtHavenIdentity();
                    });
                }

                [Fact]
                public void NoIdentityOptionsService_Throws()
                {
                    var services = new ServiceCollection();
                    var app = new ApplicationBuilder(services.BuildServiceProvider());

                    Assert.Throws<InvalidOperationException>(() =>
                    {
                        app.UseThoughtHavenIdentity();
                    });
                }
            }
        }
    }
}