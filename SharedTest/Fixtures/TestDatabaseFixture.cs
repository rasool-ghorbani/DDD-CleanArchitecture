using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace SharedTest.Fixtures
{
    public class TestDatabaseFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public TestDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            Context = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}