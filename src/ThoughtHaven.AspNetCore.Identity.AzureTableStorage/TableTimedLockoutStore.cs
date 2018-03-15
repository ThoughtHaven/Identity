using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.Azure.Storage.Table;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableTimedLockoutStore : ICrudStore<string, TimedLockout>
    {
        protected readonly TableCrudStore<string, TimedLockoutModel> ModelStore;

        public TableTimedLockoutStore(CloudStorageAccount account,
            TableRequestOptions requestOptions, TableStoreOptions storeOptions)
            : this(new TableCrudStore<string, TimedLockoutModel>(
                entityStore: BuildEntityStore(account, requestOptions, storeOptions),
                dataKeyToEntityKeys: key => CreateEntityKeys(key),
                dataToEntityKeys: model => CreateEntityKeys(model.Key)))
        { }

        protected TableTimedLockoutStore(TableCrudStore<string, TimedLockoutModel> modelStore)
        {
            this.ModelStore = Guard.Null(nameof(modelStore), modelStore);
        }

        public virtual async Task<TimedLockout> Retrieve(string key)
        {
            Guard.NullOrWhiteSpace(nameof(key), key);

            var model = await this.ModelStore.Retrieve(key).ConfigureAwait(false);

            if (model == null) { return null; }

            return new TimedLockout(model.Key, model.LastModified)
            {
                FailedAccessAttempts = model.FailedAccessAttempts,
                Expiration = model.Expiration,
            };
        }

        public virtual async Task<TimedLockout> Create(TimedLockout lockout)
        {
            Guard.Null(nameof(lockout), lockout);

            var model = new TimedLockoutModel(lockout);

            await this.ModelStore.Create(model).ConfigureAwait(false);

            return lockout;
        }

        public virtual async Task<TimedLockout> Update(TimedLockout lockout)
        {
            Guard.Null(nameof(lockout), lockout);

            var model = new TimedLockoutModel(lockout);

            await this.ModelStore.Update(model).ConfigureAwait(false);

            return lockout;
        }

        public virtual Task Delete(string key)
        {
            Guard.NullOrWhiteSpace(nameof(key), key);

            return this.ModelStore.Delete(key);
        }

        protected static TableEntityKeys CreateEntityKeys(string key)
        {
            Guard.NullOrWhiteSpace(nameof(key), key);

            return new TableEntityKeys(key, "Lockout");
        }

        protected static TableEntityStore BuildEntityStore(CloudStorageAccount account,
            TableRequestOptions requestOptions, TableStoreOptions storeOptions)
        {
            Guard.Null(nameof(account), account);
            Guard.Null(nameof(requestOptions), requestOptions);
            Guard.Null(nameof(storeOptions), storeOptions);

            return new TableEntityStore(
                table: account.CreateCloudTableClient().GetTableReference(
                    storeOptions.TimedLockoutStoreTableName),
                options: requestOptions);
        }
    }
}