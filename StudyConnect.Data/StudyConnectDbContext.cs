using System.ComponentModel;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyConnect.Data.Entities;

namespace StudyConnect.Data;

/// <summary>
/// Represents the database context for the StudyConnect application.
/// This context is responsible for managing the connection to the database and providing access to the entities in the application.
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

        // Create a Default UserRole Student
        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            URoleId = new Guid("00000000-0000-0000-0000-000000000001"),
            Name = "Student",
            Description = "Student is the default role with no rights."
        });

        // Configure User-Group relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.Groups)
            .WithOne(g => g.Owner)
            .HasForeignKey(g => g.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Group member composite key
        modelBuilder.Entity<GroupMember>()
        .HasKey(gm => new { gm.MemberId, gm.GroupId });

        // Configure Group-GroupMember relationship
        modelBuilder.Entity<Group>()
            .HasMany(g => g.GroupMembers)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure User-GroupMember relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.GroupMembers)
            .WithOne(gm => gm.Member)
            .HasForeignKey(gm => gm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMember>()
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

        // Configure ForumComment-ForumComment relationship
        modelBuilder.Entity<ForumComment>()
            .HasOne(fc => fc.ParentComment)
            .WithMany(fc => fc.Replies)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumLike>()
            .HasOne(l => l.User)
            .WithMany(u => u.ForumLikes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Like-ForumPost relationship
        modelBuilder.Entity<ForumLike>()
            .HasOne(l => l.ForumPost)
            .WithMany(p => p.ForumLikes)
            .HasForeignKey(l => l.ForumPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Like-ForumComment relationship
        modelBuilder.Entity<ForumLike>()
            .HasOne(l => l.ForumComment)
            .WithMany(c => c.ForumLikes)
            .HasForeignKey(l => l.ForumCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PostTag>()
            .HasOne(t => t.Tag)
            .WithMany(pt => pt.PostTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PostTag>()
            .HasOne(t => t.ForumPost)
            .WithMany(pt => pt.PostTags)
            .HasForeignKey(pt => pt.ForumPostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tag>().HasKey(t => t.TagId);
        modelBuilder.Entity<Tag>().Property(t => t.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Tag>().Property(t => t.Description).HasMaxLength(500);

        // Configure PostTag-ForumPost relationship
        modelBuilder.Entity<PostTag>()
            .HasKey(pt => new { pt.ForumPostId, pt.TagId });

        // Configure Unique non-key indexes
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserRole>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<MemberRole>().HasIndex(m => m.Name).IsUnique();
        modelBuilder.Entity<ForumCategory>().HasIndex(f => f.Name).IsUnique();
        modelBuilder.Entity<ForumLike>().HasIndex(l => new { l.UserId, l.ForumPostId, l.ForumCommentId }).IsUnique();
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();


        // Configure table names to match SQL schema
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<UserRole>().ToTable("UserRole");
        modelBuilder.Entity<Group>().ToTable("Group");
        modelBuilder.Entity<GroupMember>().ToTable("GroupMember");
        modelBuilder.Entity<MemberRole>().ToTable("MemberRole");
        modelBuilder.Entity<ForumCategory>().ToTable("ForumCategory");
        modelBuilder.Entity<ForumPost>().ToTable("ForumPost");
        modelBuilder.Entity<ForumComment>().ToTable("ForumComment");
        modelBuilder.Entity<ForumLike>().ToTable("ForumLike");
        modelBuilder.Entity<Tag>().ToTable("Tag");
        modelBuilder.Entity<PostTag>().ToTable("PostTag");

        modelBuilder.Entity<ForumCategory>().HasData(
            new ForumCategory
            {
                ForumCategoryId = new Guid("a3f1d8a5-1b67-4a4f-91c1-9c63c2bde914"),
                Name = "SWEN1",
                Description = "Software Entwicklung 2"
            },
            new ForumCategory
            {
                ForumCategoryId = new Guid("e5b7f442-06c6-4d7e-b6fa-4f8f2420a6e7"),
                Name = "CT-2",
                Description = "Computer Technik 2"
            },
            new ForumCategory
            {
                ForumCategoryId = new Guid("c345e8a7-8c49-4326-83e7-2657b1d149f3"),
                Name = "BSY",
                Description = "Betriebssysteme"
            },
            new ForumCategory
            {
                ForumCategoryId = new Guid("a1b2c3d4-5678-4f90-abcd-1234567890ef"),
                Name = "General",
                Description = "General Kontext"
            }
        );

        // Create a Default GroupRole Student
        modelBuilder.Entity<MemberRole>().HasData(new MemberRole
        {
            MemberRoleId = new Guid("00000000-0000-0000-0000-000000000010"),
            Name = "GroupMember",
            Description = "Is a member of a group"
        });
        modelBuilder.Entity<Tag>().HasData(
            new Tag
            {
                TagId = new Guid("dba25ed0-ea90-4de8-9b73-bedd16d15a5f"),
                Name = "Question",
                Description = "Ask your question here"
            },
            new Tag
            {
                TagId = new Guid("27a0d0a6-9df8-429e-b473-129533d460d5"),
                Name = "Looking for Group",
                Description = "Looking for study group members"
            },
            new Tag
            {
                TagId = new Guid("46fd2f68-df2d-4a0a-9137-f8556b4f132f"),
                Name = "Discussion",
                Description = "Discuss the topic here"
            },
            new Tag
            {
                TagId = new Guid("9d2f3e3f-0f58-4d55-8337-84fecd2b84d3"),
                Name = "Issue",
                Description = "Check existing problems or issues"
            }
        );

    }
    /// <summary>
    /// Gets or sets the Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the UserRoles table.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Gets or sets the Groups table.
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    /// <summary>
    /// Gets or sets the GroupMembers table.
    /// </summary>
    public DbSet<GroupMember> GroupMembers { get; set; }

    /// <summary>
    /// Gets or sets the MemberRoles table.
    /// </summary>
    public DbSet<MemberRole> MemberRoles { get; set; }

    /// <summary>
    /// Gets or sets the ForumCategories table.
    /// </summary>
    public DbSet<ForumCategory> ForumCategories { get; set; }

    /// <summary>
    /// Gets or sets the ForumPosts table.
    /// </summary>
    public DbSet<ForumPost> ForumPosts { get; set; }

    /// <summary>
    /// Gets or sets the ForumComments table.
    /// </summary>
    public DbSet<ForumComment> ForumComments { get; set; }
    public DbSet<ForumLike> ForumLikes { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public DbSet<PostTag> PostTags { get; set; }
}
