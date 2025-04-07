using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyConnect.Core.Entities;

namespace StudyConnect.Data;

/// <summary>
/// Represents the database context for the StudyConnect application.
/// This context is used to interact with the database.
/// It inherits from DbContext and is configured to use SQL Server.
/// </summary>
public class StudyConnectDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudyConnectDbContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="configuration"></param>
    public StudyConnectDbContext(DbContextOptions<StudyConnectDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Configures the database context options.
    /// If the options have not been configured, it sets the connection string to the default connection string from the configuration.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    /// <summary>
    /// DbSet representing the Users table in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }
}