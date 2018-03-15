using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using ThoughtHaven.Azure.Storage.Table;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Stores.Fakes
{
    public class FakeTimedLockoutCrudStore : TableCrudStore<string, TimedLockoutModel>
    {
        public FakeTimedLockoutCrudStore()
            : base(entityStore: new TableEntityStore(
                new CloudTable(new Uri("https://example.com/table")),
                new TableRequestOptions()),
                  dataKeyToEntityKeys: k => null,
                  dataToEntityKeys: m => null)
        { }

        public string Retrieve_InputKey;
        public TimedLockoutModel Retrieve_Output = new TimedLockoutModel()
        {
            Key = "key",
            LastModified = DateTimeOffset.UtcNow,
            FailedAccessAttempts = 1,
            Expiration = DateTimeOffset.UtcNow.AddDays(1),
        };
        public override Task<TimedLockoutModel> Retrieve(string key)
        {
            this.Retrieve_InputKey = key;

            return Task.FromResult(this.Retrieve_Output);
        }

        public TimedLockoutModel Create_InputModel;
        public override Task<TimedLockoutModel> Create(TimedLockoutModel model)
        {
            this.Create_InputModel = model;

            return Task.FromResult(model);
        }

        public TimedLockoutModel Update_InputModel;
        public override Task<TimedLockoutModel> Update(TimedLockoutModel model)
        {
            this.Update_InputModel = model;

            return Task.FromResult(model);
        }

        public string Delete_InputKey;
        public override Task Delete(string key)
        {
            this.Delete_InputKey = key;

            return Task.CompletedTask;
        }
    }
}