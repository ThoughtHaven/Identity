using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Stores.Fakes
{
    public class FakeTableUserStore : TableUserStore<User>
    {
        public FakeTableUserStore(CloudStorageAccount account,
            TableRequestOptions requestOptions, TableStoreOptions storeOptions)
            : base(account, requestOptions, storeOptions)
        { }

        new public static TableEntityKeys CreateEntityKeys(UserKey key) =>
            TableUserStore<User>.CreateEntityKeys(key);
    }
}