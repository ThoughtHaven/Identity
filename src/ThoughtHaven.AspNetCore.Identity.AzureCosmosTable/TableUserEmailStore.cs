using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using ThoughtHaven.AspNetCore.Identity.Emails;
using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Azure.Cosmos.Table;
using ThoughtHaven.Data;
using System.Linq;
using ThoughtHaven.Contacts;
using System;

namespace ThoughtHaven.AspNetCore.Identity.AzureCosmosTable
{
    public class TableUserEmailStore<TUser>
        : TableUserStore<TUser>, IRetrieveOperation<EmailAddress, TUser>
        where TUser : class, IUserKey, IUserEmail, new()
    {
        public TableUserEmailStore(TableStoreOptions options) : base(options) { }

        protected TableUserEmailStore(TableEntityStore entityStore) : base(entityStore) { }

        protected TableUserEmailStore(TableEntityStore entityStore,
            Func<UserKey, TableEntityKeys> userKeyToEntityKeys,
            Func<TUser, TableEntityKeys> userToEntityKeys)
            : base(entityStore, userKeyToEntityKeys, userToEntityKeys)
        { }

        public virtual async Task<TUser?> Retrieve(EmailAddress email)
        {
            Guard.Null(nameof(email), email);

            await this.EntityStore.ExistenceTester.EnsureExists(this.EntityStore.Table)
                .ConfigureAwait(false);

            var query = new TableQuery().Where(TableQuery.GenerateFilterCondition(
                nameof(IUserEmail.Email), QueryComparisons.Equal, email.Value.ToUpperInvariant()));

            var segment = await this.EntityStore.Table.ExecuteQuerySegmentedAsync(query,
                new TableContinuationToken(), this.EntityStore.Options, operationContext: null);

            var entity = segment?.Results.SingleOrDefault();

            if (entity == null) { return null; }

            var user = new TUser();
            TableEntity.ReadUserObject(user, entity.Properties, operationContext: null);
            return user;
        }
    }
}