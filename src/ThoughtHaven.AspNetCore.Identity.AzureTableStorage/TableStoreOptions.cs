namespace ThoughtHaven.AspNetCore.Identity.Stores
{
    public class TableStoreOptions
    {
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
            set { this._timedLockoutStoreTableName = Guard.NullOrWhiteSpace(nameof(value), value); }
        }
    }
}