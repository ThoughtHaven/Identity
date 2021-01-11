using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.AzureCosmosTable.Fakes
{
    public class FakeTableUserStore : TableUserStore<User>
    {
        public FakeTableUserStore(TableStoreOptions options) : base(options) { }

        new public static TableEntityKeys CreateEntityKeys(UserKey key) =>
            TableUserStore<User>.CreateEntityKeys(key);
    }
}