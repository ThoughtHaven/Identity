using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
using ThoughtHaven.Security.SingleUseTokens.AzureTableStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableStorageIdentityOptions : IdentityOptions
    {
        public TableRequestOptions TableRequest
        {
            get { return this.TableStore.TableRequest; }
            set
            {
                Guard.Null(nameof(value), value);

                this.TableStore.TableRequest = value;
                this.SingleUseToken.TableRequest = value;
            }
        }

        private TableStoreOptions _tableStore;
        public TableStoreOptions TableStore
        {
            get { return this._tableStore; }
            set { this._tableStore = Guard.Null(nameof(value), value); }
        }

        private TableSingleUseTokenOptions _singleUseToken;
        public virtual TableSingleUseTokenOptions SingleUseToken
        {
            get { return this._singleUseToken; }
            set { this._singleUseToken = Guard.Null(nameof(value), value); }
        }

        public TableStorageIdentityOptions(string storageAccountConnectionString)
            : this(tableStore: new TableStoreOptions(Guard.NullOrWhiteSpace(
                nameof(storageAccountConnectionString), storageAccountConnectionString)),
                  singleUseToken: new TableSingleUseTokenOptions(
                      storageAccountConnectionString))
        { }

        public TableStorageIdentityOptions(TableStoreOptions tableStore,
            TableSingleUseTokenOptions singleUseToken)
        {
            this._tableStore = Guard.Null(nameof(tableStore), tableStore);
            this._singleUseToken = Guard.Null(nameof(singleUseToken), singleUseToken);
        }
    }
}