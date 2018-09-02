using Microsoft.WindowsAzure.Storage.Table;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage
{
    public class TableStoreConfiguration
    {
        public string StorageAccountConnectionString { get; }

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

        public TableStoreConfiguration(string storageAccountConnectionString)
        {
            this.StorageAccountConnectionString = Guard.NullOrWhiteSpace(
                nameof(storageAccountConnectionString), storageAccountConnectionString);
        }
    }
}