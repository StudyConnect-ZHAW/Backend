using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyConnect.Data.Entities;

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
    /// Configures the model for the database.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.URole_ID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure table names to match SQL schema
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<UserRole>().ToTable("UserRole");
        modelBuilder.Entity<Group>().ToTable("Group");
        modelBuilder.Entity<GroupMembers>().ToTable("GroupMembers");
        modelBuilder.Entity<MemberRole>().ToTable("MemberRole");
        modelBuilder.Entity<ForumCategory>().ToTable("ForumCategory");
        modelBuilder.Entity<ForumPost>().ToTable("ForumPost");
        modelBuilder.Entity<ForumComment>().ToTable("ForumComment");
    }

    /// <summary>
    /// DbSet representing the Users table in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// DbSet representing the UserRoles table in the database.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// DbSet representing the Groups table in the database.
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// DbSet representing the GroupMembers table in the database.
    /// </summary>
    public DbSet<GroupMembers> GroupMembers { get; set; }

    /// <summary>
    /// DbSet representing the MemberRoles table in the database.
    /// </summary>
    public DbSet<MemberRole> MemberRoles { get; set; }

    /// <summary>
    /// DbSet representing the ForumCategories table in the database.
    /// </summary>
    public DbSet<ForumCategory> ForumCategories { get; set; }

    /// <summary>
    /// DbSet representing the ForumPosts table in the database.
    /// </summary>
    public DbSet<ForumPost> ForumPosts { get; set; }

    /// <summary>
    /// DbSet representing the ForumComments table in the database.
    /// </summary>
    public DbSet<ForumComment> ForumComments { get; set; }
}
