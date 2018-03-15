using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven;
using ThoughtHaven.AspNetCore.Identity.Stores;
using ThoughtHaven.Security.SingleUseTokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public class TableStorageIdentityOptions : IdentityOptions
    {
        private TableStoreOptions _tableStore = new TableStoreOptions();
        public TableStoreOptions TableStore
        {
            get { return this._tableStore; }
            set { this._tableStore = Guard.Null(nameof(value), value); }
        }

        private TableRequestOptions _tableRequest = new TableRequestOptions();
        public TableRequestOptions TableRequest
        {
            get { return this._tableRequest; }
            set
            {
                this._tableRequest = Guard.Null(nameof(value), value);
                this.SingleUseToken.TableRequest = value;
            }
        }

        private TableSingleUseTokenOptions _singleUseToken = new TableSingleUseTokenOptions();
        public virtual TableSingleUseTokenOptions SingleUseToken
        {
            get { return this._singleUseToken; }
            set { this._singleUseToken = Guard.Null(nameof(value), value); }
        }
    }
}