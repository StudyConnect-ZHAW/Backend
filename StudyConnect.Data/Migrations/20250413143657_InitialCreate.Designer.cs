﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyConnect.Data;

#nullable disable

namespace StudyConnect.Data.Migrations
{
    [DbContext(typeof(StudyConnectDbContext))]
    [Migration("20250413143657_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumCategory", b =>
                {
                    b.Property<Guid>("ForumCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ForumCategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ForumCategory", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumComment", b =>
                {
                    b.Property<Guid>("ForumCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DislikeCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ForumPostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPinned")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ReplyCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("ForumCommentId");

                    b.HasIndex("ForumPostId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserGuid");

                    b.ToTable("ForumComment", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumPost", b =>
                {
                    b.Property<Guid>("ForumPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DislikeCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ForumCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPinned")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("ForumPostId");

                    b.HasIndex("ForumCategoryId");

                    b.HasIndex("UserGuid");

                    b.ToTable("ForumPost", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.Group", b =>
                {
                    b.Property<Guid>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Visibility")
                        .HasColumnType("bit");

                    b.HasKey("GroupId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.GroupMembers", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MemberRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MemberId", "GroupId");

                    b.HasIndex("GroupId");

                    b.HasIndex("MemberRoleId");

                    b.ToTable("GroupMembers", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.MemberRole", b =>
                {
                    b.Property<Guid>("MemberRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MemberRoleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MemberRole", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.User", b =>
                {
                    b.Property<Guid>("UserGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("URoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserGuid");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("URoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid>("URoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("URoleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumComment", b =>
                {
                    b.HasOne("StudyConnect.Data.Entities.ForumPost", "ForumPost")
                        .WithMany("ForumComments")
                        .HasForeignKey("ForumPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyConnect.Data.Entities.ForumComment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("StudyConnect.Data.Entities.User", "User")
                        .WithMany("ForumComments")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ForumPost");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumPost", b =>
                {
                    b.HasOne("StudyConnect.Data.Entities.ForumCategory", "ForumCategory")
                        .WithMany("ForumPosts")
                        .HasForeignKey("ForumCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudyConnect.Data.Entities.User", "User")
                        .WithMany("ForumPosts")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForumCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.Group", b =>
                {
                    b.HasOne("StudyConnect.Data.Entities.User", "Owner")
                        .WithMany("Groups")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.GroupMembers", b =>
                {
                    b.HasOne("StudyConnect.Data.Entities.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyConnect.Data.Entities.User", "Member")
                        .WithMany("GroupMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudyConnect.Data.Entities.MemberRole", "MemberRole")
                        .WithMany("GroupMembers")
                        .HasForeignKey("MemberRoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Member");

                    b.Navigation("MemberRole");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.User", b =>
                {
                    b.HasOne("StudyConnect.Data.Entities.UserRole", "URole")
                        .WithMany("Users")
                        .HasForeignKey("URoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("URole");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumCategory", b =>
                {
                    b.Navigation("ForumPosts");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumComment", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.ForumPost", b =>
                {
                    b.Navigation("ForumComments");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.Group", b =>
                {
                    b.Navigation("GroupMembers");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.MemberRole", b =>
                {
                    b.Navigation("GroupMembers");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.User", b =>
                {
                    b.Navigation("ForumComments");

                    b.Navigation("ForumPosts");

                    b.Navigation("GroupMembers");

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("StudyConnect.Data.Entities.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
