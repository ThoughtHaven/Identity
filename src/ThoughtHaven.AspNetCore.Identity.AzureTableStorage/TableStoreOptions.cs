using Microsoft.WindowsAzure.Storage.Table;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableStoreOptions
    {
        private string _storageAccountConnectionString;
        public virtual string StorageAccountConnectionString
        {
            get { return this._storageAccountConnectionString; }
            set
            {
                this._storageAccountConnectionString = Guard.NullOrWhiteSpace(nameof(value),
                    value);
            }
        }

        private TableRequestOptions _tableRequest = new TableRequestOptions();
        public TableRequestOptions TableRequest
        {
            get { return this._tableRequest; }
            set
            {
                this._tableRequest = Guard.Null(nameof(value), value);
            }
        }

        private string _userStoreTableName = "IdentityUsers";
        public virtual string UserStoreTableName
        {
            get { return this._userStoreTableName; }
            set { this._userStoreTableName = Guard.NullOrWhiteSpace(nameof(value), value); }
        }

        private string _timedLockoutStoreTableName = "IdentityTimedLockouts";
        public virtual string TimedLockoutStoreTableName
        {
            get { return this._timedLockoutStoreTableName; }
            set
            {
                this._timedLockoutStoreTableName = Guard.NullOrWhiteSpace(nameof(value),
                    value);
            }
        }

        public TableStoreOptions(string storageAccountConnectionString)
        {
            this._storageAccountConnectionString = Guard.NullOrWhiteSpace(
                nameof(storageAccountConnectionString), storageAccountConnectionString);
        }
    }
}