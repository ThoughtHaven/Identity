using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Lockouts;
using ThoughtHaven.Azure.Storage.Table;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableTimedLockoutStore : ICrudStore<string, TimedLockout>
    {
        protected readonly TableCrudStore<string, TimedLockoutModel> ModelStore;

        public TableTimedLockoutStore(TableStoreOptions options)
            : this(new TableCrudStore<string, TimedLockoutModel>(
                entityStore: BuildEntityStore(options),
                dataKeyToEntityKeys: key => CreateEntityKeys(key),
                dataToEntityKeys: model => CreateEntityKeys(model.Key)))
        { }

        protected TableTimedLockoutStore(
            TableCrudStore<string, TimedLockoutModel> modelStore)
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

        protected static TableEntityStore BuildEntityStore(TableStoreOptions options)
        {
            Guard.Null(nameof(options), options);

            return new TableEntityStore(
                table: CloudStorageAccount.Parse(options.StorageAccountConnectionString)
                    .CreateCloudTableClient().GetTableReference(
                        options.TimedLockoutStoreTableName),
                options: options.TableRequest);
        }
    }
}