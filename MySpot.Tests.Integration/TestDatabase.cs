using Microsoft.EntityFrameworkCore;
using MySpot.Infrastructure.DAL;
using System;

namespace MySpot.Tests.Integration;

internal class TestDatabase : IDisposable
{
    public MySpotDbContext Context { get; }

    public TestDatabase()
    {

        var options = new OptionsProvider().Get<PostgresOptions>("postgres");
        Context = new MySpotDbContext(new DbContextOptionsBuilder<MySpotDbContext>()
            .UseNpgsql(options.ConnectionString).Options);
    }

    //wywolane za zatrzyaniem kazdego testu
    //po kazdym tescie
    public void Dispose()
    {
        //usuniecie bazy po testach
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}