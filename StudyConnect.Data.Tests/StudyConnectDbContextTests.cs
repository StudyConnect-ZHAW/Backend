using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace StudyConnect.Data.Tests
{
    public class StudyConnectDbContextTests
    {
        [Fact]
        public async Task CanAddAndGetUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudyConnectDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            using (var context = new StudyConnectDbContext(options, configuration))
            {
                var userRole = new UserRole { Name = "TestRole" };
                context.UserRoles.Add(userRole);
                await context.SaveChangesAsync();

                var user = new User { 
                    UserGuid = Guid.NewGuid(),
                    Email = "test@example.com",
                    FirstName = "Test",
                    LastName = "User",
                    URole = userRole
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                // Act
                var retrievedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");

                // Assert
                Assert.NotNull(retrievedUser);
                retrievedUser.Email.Should().Be("test@example.com");
            }
        }
    }
}