using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Azure.Storage.Table;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableUserStore<TUser> : TableCrudStore<UserKey, TUser>
        where TUser : class, IUserKey, new()
    {
        public TableUserStore(CloudStorageAccount account, TableRequestOptions requestOptions,
            TableStoreOptions storeOptions)
            : this(BuildEntityStore(account, requestOptions, storeOptions))
        { }

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

        protected static TableEntityStore BuildEntityStore(CloudStorageAccount account,
            TableRequestOptions requestOptions, TableStoreOptions storeOptions)
        {
            Guard.Null(nameof(account), account);
            Guard.Null(nameof(requestOptions), requestOptions);
            Guard.Null(nameof(storeOptions), storeOptions);

            return new TableEntityStore(
                account.CreateCloudTableClient().GetTableReference(
                    storeOptions.UserStoreTableName),
                options: requestOptions);
        }
    }
}