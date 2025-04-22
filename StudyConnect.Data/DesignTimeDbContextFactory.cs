using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace StudyConnect.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StudyConnectDbContext>
    {
        public StudyConnectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudyConnectDbContext>();

            DotNetEnv.Env.Load("../.env");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../StudyConnect.API"))// This ensures the path is correct
                .AddJsonFile("appsettings.Development.json", optional:false) // Add your configuration file
                .AddEnvironmentVariables() // Optionally add environment variables
                .Build(); // This is the method that actually creates the IConfiguration

            // Get the connection string from the configuration
            var rawConnectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");
            var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD"); 

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Environment variable 'DB_PASSWORD' is not set.");
            }

            var connectionString = rawConnectionString.Replace("${MSSQL_SA_PASSWORD}", password);

            // Configure the DbContext with the connection string
            optionsBuilder.UseSqlServer(connectionString);

            return new StudyConnectDbContext(optionsBuilder.Options, configuration);
        }
    }
}
