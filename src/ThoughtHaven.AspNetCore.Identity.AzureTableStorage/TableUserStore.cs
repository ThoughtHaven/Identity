using Microsoft.WindowsAzure.Storage;
using System;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Azure.Storage.Table;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableUserStore<TUser> : TableCrudStore<UserKey, TUser>
        where TUser : class, IUserKey, new()
    {
        public TableUserStore(TableStoreOptions options)
            : this(BuildEntityStore(options)) { }

        protected TableUserStore(TableEntityStore entityStore)
            : this(entityStore,
                  userKeyToEntityKeys: key => CreateEntityKeys(key),
                  userToEntityKeys: user => CreateEntityKeys(user.Key()))
        { }

        protected TableUserStore(TableEntityStore entityStore,
            Func<UserKey, TableEntityKeys> userKeyToEntityKeys,
            Func<TUser, TableEntityKeys> userToEntityKeys)
            : base(entityStore, userKeyToEntityKeys, userToEntityKeys)
        { }

        protected static TableEntityKeys CreateEntityKeys(UserKey key)
        {
            Guard.Null(nameof(key), key);

            return new TableEntityKeys(key.Value, "User");
        }

        protected static TableEntityStore BuildEntityStore(TableStoreOptions options)
        {
            Guard.Null(nameof(options), options);

            return new TableEntityStore(
                table: CloudStorageAccount.Parse(options.StorageAccountConnectionString)
                    .CreateCloudTableClient().GetTableReference(options.UserStoreTableName),
                options: options.TableRequest);
        }
    }
}