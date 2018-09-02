using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.AzureTableStorage;
using ThoughtHaven.Security.SingleUseTokens.AzureTableStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableIdentityConfiguration : IdentityOptions
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

        private TableStoreConfiguration _tableStore;
        public TableStoreConfiguration TableStore
        {
            get { return this._tableStore; }
            set { this._tableStore = Guard.Null(nameof(value), value); }
        }

        private TableSingleUseTokenConfiguration _singleUseToken;
        public virtual TableSingleUseTokenConfiguration SingleUseToken
        {
            get { return this._singleUseToken; }
            set { this._singleUseToken = Guard.Null(nameof(value), value); }
        }

        public TableIdentityConfiguration(string storageAccountConnectionString)
            : this(tableStore: new TableStoreConfiguration(Guard.NullOrWhiteSpace(
                nameof(storageAccountConnectionString), storageAccountConnectionString)),
                  singleUseToken: new TableSingleUseTokenConfiguration(
                      storageAccountConnectionString))
        { }

        public TableIdentityConfiguration(TableStoreConfiguration tableStore,
            TableSingleUseTokenConfiguration singleUseToken)
        {
            this._tableStore = Guard.Null(nameof(tableStore), tableStore);
            this._singleUseToken = Guard.Null(nameof(singleUseToken), singleUseToken);
        }
    }
}