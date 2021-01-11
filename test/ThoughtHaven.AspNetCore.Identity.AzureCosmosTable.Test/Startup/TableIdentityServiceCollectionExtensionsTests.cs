using System;
using ThoughtHaven.AspNetCore.Identity;
using ThoughtHaven.AspNetCore.Identity.AzureCosmosTable;
using ThoughtHaven.Security.SingleUseTokens;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableIdentityServiceCollectionExtensionsTests
    {
        public class AddThoughtHavenIdentityMethod
        {
            public class TUserGeneric
            {
                public class ServicesAndOptionsOverload
                {
                    [Fact]
                    public void NullServices_Throws()
                    {
                        IServiceCollection? services = null;

                        Assert.Throws<ArgumentNullException>("services", () =>
                        {
                            services!.AddThoughtHavenIdentity<User>(
                                options: Options());
                        });
                    }

                    [Fact]
                    public void NullOptions_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("options", () =>
                        {
                            Services().AddThoughtHavenIdentity<User>(options: null!);
                        });
                    }

                    [Fact]
                    public void WhenCalled_AddsOptions()
                    {
                        var services = Services();
                        var options = Options();

                        services.AddThoughtHavenIdentity<User>(options);

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<TableIdentityOptions>();

                        Assert.Equal(options, service);
                    }

                    [Fact]
                    public void WhenCalled_AddsTableStoreOptions()
                    {
                        var services = Services();
                        var options = Options();

                        services.AddThoughtHavenIdentity<User>(options);

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<TableStoreOptions>();

                        Assert.Equal(options.TableStore, service);
                    }

                    [Fact]
                    public void WhenCalled_AddsSingleUseTokens()
                    {
                        var services = Services();

                        services.AddThoughtHavenIdentity<User>(Options());

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<ISingleUseTokenService>();

                        Assert.NotNull(service);
                    }

                    [Fact]
                    public void WhenCalled_AddsThoughtHavenIdentity()
                    {
                        var services = Services();

                        services.AddThoughtHavenIdentity<User>(Options());

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<IIdentityService<User>>();

                        Assert.NotNull(service);
                    }
                }
            }
        }

        private static IServiceCollection Services() => new ServiceCollection();
        private static TableIdentityOptions Options() =>
            new TableIdentityOptions("UseDevelopmentStorage=true;");
    }
}