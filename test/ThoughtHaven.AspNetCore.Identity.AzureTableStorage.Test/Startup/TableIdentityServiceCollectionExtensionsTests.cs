using System;
using ThoughtHaven.AspNetCore.Identity;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
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
                public class ServicesAndConfigurationOverload
                {
                    [Fact]
                    public void NullServices_Throws()
                    {
                        IServiceCollection services = null;

                        Assert.Throws<ArgumentNullException>("services", () =>
                        {
                            services.AddThoughtHavenIdentity<User>(
                                configuration: Configuration());
                        });
                    }

                    [Fact]
                    public void NullConfiguration_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("configuration", () =>
                        {
                            Services().AddThoughtHavenIdentity<User>(configuration: null);
                        });
                    }

                    [Fact]
                    public void WhenCalled_AddsOptions()
                    {
                        var services = Services();
                        var options = Configuration();

                        services.AddThoughtHavenIdentity<User>(options);

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<TableIdentityConfiguration>();

                        Assert.Equal(options, service);
                    }

                    [Fact]
                    public void WhenCalled_AddsTableStoreOptions()
                    {
                        var services = Services();
                        var options = Configuration();

                        services.AddThoughtHavenIdentity<User>(options);

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<TableStoreConfiguration>();

                        Assert.Equal(options.TableStore, service);
                    }

                    [Fact]
                    public void WhenCalled_AddsSingleUseTokens()
                    {
                        var services = Services();

                        services.AddThoughtHavenIdentity<User>(Configuration());

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<ISingleUseTokenService>();

                        Assert.NotNull(service);
                    }

                    [Fact]
                    public void WhenCalled_AddsThoughtHavenIdentity()
                    {
                        var services = Services();

                        services.AddThoughtHavenIdentity<User>(Configuration());

                        var service = services.BuildServiceProvider()
                            .GetRequiredService<IIdentityService<User>>();

                        Assert.NotNull(service);
                    }
                }
            }
        }

        private static IServiceCollection Services() => new ServiceCollection();
        private static TableIdentityConfiguration Configuration() =>
            new TableIdentityConfiguration("UseDevelopmentStorage=true;");
    }
}