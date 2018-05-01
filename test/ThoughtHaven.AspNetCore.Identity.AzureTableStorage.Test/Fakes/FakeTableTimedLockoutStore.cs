using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage.Fakes
{
    public class FakeTableTimedLockoutStore : TableTimedLockoutStore
    {
        new public FakeTimedLockoutCrudStore ModelStore =>
            (FakeTimedLockoutCrudStore)base.ModelStore;

        public FakeTableTimedLockoutStore(FakeTimedLockoutCrudStore modelStore)
            : base(modelStore)
        { }

        new public static TableEntityKeys CreateEntityKeys(string key) =>
            TableTimedLockoutStore.CreateEntityKeys(key);
    }
}