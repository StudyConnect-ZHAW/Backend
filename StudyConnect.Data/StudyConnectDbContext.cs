using System.ComponentModel;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyConnect.Data.Entities;

namespace StudyConnect.Data;

/// <summary>
/// Represents the database context for the StudyConnect application.
/// /// This context is responsible for managing the connection to the database and providing access to the entities in the application.
/// It includes DbSet properties for each entity type, allowing for CRUD operations and LINQ queries.
/// </summary>
public class StudyConnectDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudyConnectDbContext"/> class.
    /// </summary>
    /// <param name="options"> The options to configure the context.</param>
    /// <param name="configuration"> The configuration used to set up the context.</param>
    public StudyConnectDbContext(DbContextOptions<StudyConnectDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Configures the database context options.
    /// If the options have not been configured, it sets the connection string to the default connection string from the configuration.
    /// </summary>
    /// <param name="optionsBuilder"> The options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    /// <summary>
    /// Configures the model for the database context.
    /// This includes setting up relationships, keys, and indexes for the entities in the context.
    /// It also configures the table names to match the SQL schema.
    /// </summary>
    /// <param name="modelBuilder"> The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User-Role relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.URole)
            .WithMany(ur => ur.Users)
            .HasForeignKey("URoleId")
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-Group relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.Groups)
            .WithOne(g => g.Owner)
            .HasForeignKey(g => g.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Group member composite key
        modelBuilder.Entity<GroupMembers>()
        .HasKey(gm => new { gm.MemberId, gm.GroupId });

        // Configure Group-GroupMembers relationship
        modelBuilder.Entity<Group>()
            .HasMany(g => g.GroupMembers)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure User-GroupMembers relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.GroupMembers)
            .WithOne(gm => gm.Member)
            .HasForeignKey(gm => gm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMembers>()
            .HasOne(gm => gm.MemberRole)
            .WithMany(mr => mr.GroupMembers)
            .HasForeignKey(gm => gm.MemberRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User-ForumPost relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.ForumPosts)
            .WithOne(fp => fp.User)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Category-ForumPost relationship
        modelBuilder.Entity<ForumPost>()
            .HasOne(fp => fp.ForumCategory)
            .WithMany(fc => fc.ForumPosts)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ForumPost-ForumCategory relationship
        modelBuilder.Entity<ForumCategory>()
            .HasMany(fc => fc.ForumPosts)
            .WithOne(fp => fp.ForumCategory)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ForumPost-ForumComment relationship
        modelBuilder.Entity<ForumPost>()
            .HasMany(fp => fp.ForumComments)
            .WithOne(fc => fc.ForumPost)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure User-ForumComment relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.ForumComments)
            .WithOne(fc => fc.User)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Unique non-key indexes
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserRole>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<MemberRole>().HasIndex(m => m.Name).IsUnique();
        modelBuilder.Entity<ForumCategory>().HasIndex(f => f.Name).IsUnique();

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

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMembers> GroupMembers { get; set; }
    public DbSet<MemberRole> MemberRoles { get; set; }
    public DbSet<ForumCategory> ForumCategories { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<ForumComment> ForumComments { get; set; }
}
