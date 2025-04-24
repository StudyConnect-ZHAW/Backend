using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudyConnect.Data.Tests;

public class TestUtils
{
    public static DbContextOptions<StudyConnectDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<StudyConnectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    public static IConfiguration CreateNewConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
