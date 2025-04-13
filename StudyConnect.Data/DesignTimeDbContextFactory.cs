using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StudyConnect.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StudyConnectDbContext>
    {
        public StudyConnectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudyConnectDbContext>();

            // Correctly create the ConfigurationBuilder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // This ensures the path is correct
                .AddJsonFile("appsettings.json") // Add your configuration file
                .AddEnvironmentVariables() // Optionally add environment variables
                .Build(); // This is the method that actually creates the IConfiguration

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

      // Configure the DbContext with the connection string
      optionsBuilder.UseSqlServer(connectionString);

            return new StudyConnectDbContext(optionsBuilder.Options, configuration);
        }
    }
}
