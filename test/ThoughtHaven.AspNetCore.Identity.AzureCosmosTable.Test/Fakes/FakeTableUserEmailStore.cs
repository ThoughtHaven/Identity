namespace ThoughtHaven.AspNetCore.Identity.AzureCosmosTable.Fakes
{
    public class FakeTableUserEmailStore : TableUserEmailStore<User>
    {
        new public FakeTableEntityStore EntityStore =>
            (FakeTableEntityStore)base.EntityStore;

        public FakeTableUserEmailStore(FakeTableEntityStore entityStore)
            : base(entityStore) { }
    }
}