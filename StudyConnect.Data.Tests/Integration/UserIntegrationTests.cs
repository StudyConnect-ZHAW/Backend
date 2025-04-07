using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyConnect.Data;

/// <summary>
/// Integration tests for the User entity
/// using an in-memory database to simulate the database environment.
/// These tests verify the functionality of the User entity in the database.
/// </summary>
public class UserIntegrationTests
{
    [Fact]
    public void AddUser_ShouldAddUserToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<StudyConnectDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var configuration = new ConfigurationBuilder().Build();

        Guid userGuid = Guid.NewGuid();

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var user = new User
            {
                UserGuid = userGuid,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            // Act
            context.Users.Add(user);
            context.SaveChanges();
        }

        // Assert
        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var addedUser = context.Users.FirstOrDefault(u => u.UserGuid == userGuid);
            Assert.NotNull(addedUser);
            Assert.Equal("John", addedUser.FirstName);
        }
    }
}