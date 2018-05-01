namespace ThoughtHaven.AspNetCore.Identity.AzureTableStorage.Fakes
{
    public class FakeTableUserEmailStore : TableUserEmailStore<User>
    {
        new public FakeTableEntityStore EntityStore =>
            (FakeTableEntityStore)base.EntityStore;

        public FakeTableUserEmailStore(FakeTableEntityStore entityStore)
            : base(entityStore) { }
    }
}