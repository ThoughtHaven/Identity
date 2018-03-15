using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity;
using ThoughtHaven.AspNetCore.Identity.Stores;
using ThoughtHaven.Security.SingleUseTokens;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableStorageIdentityServiceCollectionExtensionsTests
    {
        public class AddThoughtHavenIdentityMethod
        {
            public class PrimaryOverload
            {
                [Fact]
                public void NullServices_Throws()
                {
                    IServiceCollection services = null;

                    Assert.Throws<ArgumentNullException>("services", () =>
                    {
                        services.AddThoughtHavenIdentity<User>(
                            storageAccountConnectionString: ConnectionString());
                    });
                }

                [Fact]
                public void NullStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentNullException>("storageAccountConnectionString", () =>
                    {
                        Services().AddThoughtHavenIdentity<User>(
                            storageAccountConnectionString: null);
                    });
                }

                [Fact]
                public void EmptyStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        Services().AddThoughtHavenIdentity<User>(
                            storageAccountConnectionString: "");
                    });
                }

                [Fact]
                public void WhiteSpaceStorageAccountConnectionString_Throws()
                {
                    Assert.Throws<ArgumentException>("storageAccountConnectionString", () =>
                    {
                        Services().AddThoughtHavenIdentity<User>(
                            storageAccountConnectionString: " ");
                    });
                }

                [Fact]
                public void DefaultOptions_AddsOptions()
                {
                    var services = Services();

                    services.AddThoughtHavenIdentity<User>(ConnectionString());

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<TableStorageIdentityOptions>();

                    Assert.NotNull(service);
                }

                [Fact]
                public void OptionsNull_AddsOptions()
                {
                    var services = Services();

                    services.AddThoughtHavenIdentity<User>(ConnectionString(), options: null);

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<TableStorageIdentityOptions>();

                    Assert.NotNull(service);
                }

                [Fact]
                public void OptionsValue_AddsOptions()
                {
                    var services = Services();
                    var options = Options();

                    services.AddThoughtHavenIdentity<User>(ConnectionString(), options);

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<TableStorageIdentityOptions>();

                    Assert.Equal(options, service);
                }

                [Fact]
                public void WhenCalled_AddsTableStoreOptions()
                {
                    var services = Services();
                    var options = Options();

                    services.AddThoughtHavenIdentity<User>(ConnectionString(), options);

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<TableStoreOptions>();

                    Assert.Equal(options.TableStore, service);
                }

                [Fact]
                public void WhenCalled_AddsTableRequestOptions()
                {
                    var services = Services();
                    var options = Options();

                    services.AddThoughtHavenIdentity<User>(ConnectionString(), options);

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<TableRequestOptions>();

                    Assert.Equal(options.TableRequest, service);
                }

                [Fact]
                public void WhenCalled_AddsCloudStorageAccount()
                {
                    var services = Services();

                    services.AddThoughtHavenIdentity<User>(ConnectionString());

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<CloudStorageAccount>();

                    Assert.NotNull(service);
                }

                [Fact]
                public void WhenCalled_AddsSingleUseTokens()
                {
                    var services = Services();

                    services.AddThoughtHavenIdentity<User>(ConnectionString());

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<ISingleUseTokenService>();

                    Assert.NotNull(service);
                }

                [Fact]
                public void WhenCalled_AddsThoughtHavenIdentityBase()
                {
                    var services = Services();

                    services.AddThoughtHavenIdentity<User>(ConnectionString());

                    var service = services.BuildServiceProvider()
                        .GetRequiredService<IIdentityService<User>>();

                    Assert.NotNull(service);
                }
            }
        }

        private static IServiceCollection Services() => new ServiceCollection();
        private static string ConnectionString() => "UseDevelopmentStorage=true;";
        private static TableStorageIdentityOptions Options() =>
            new TableStorageIdentityOptions();
    }
}