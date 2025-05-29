using Microsoft.Identity.Client;
using Xunit;
using System.Net.Http;
using Xunit.Abstractions;

namespace StudyConnect.API.IntegrationTests;

public class UnitTestAPI
{

    private static string _accessToken;

    private readonly ITestOutputHelper _output;

    public UnitTestAPI(ITestOutputHelper output)
    {
        _output = output;
        InitializeAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeAsync()
    {
        _accessToken = await GetTestUserAccessToken();
    }

    private static async Task<string> GetTestUserAccessToken()
    {
        // Configuration 
        string authority = "https://.ciamlogin.com//v2.0";
        string clientId = "";
        string[] scopes = new[] {"openid","profile", "api:///StudyConnectAPI"};
        string username = "test@studyconnectaoutlook.onmicrosoft.com";
        string password = "";

        var app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(authority)
            .Build();

        var securePassword = new System.Security.SecureString();
        foreach (char c in password)
        {
            securePassword.AppendChar(c);
        }

        var result = await app.AcquireTokenByUsernamePassword(scopes, username, securePassword)
            .ExecuteAsync();

        Console.WriteLine($"Access Token: {result.AccessToken}");
        Console.WriteLine("-----------------------------------------------------------------------------------");

        return result.AccessToken;
    }

    [Fact]
    public async Task ReadAccessToken()
    {
        // Arrange

        // Act

        // Assert
        Assert.NotNull(_accessToken);
        Assert.NotEmpty(_accessToken);
    }

    //Add User with API and check if it exists
    [Fact]
    public async Task CreateUserAndCheckIfExists()
    {
        // Arrange
        HttpResponseMessage respones;

        // Act
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
            respones = await client.PostAsync("http://localhost:8080/api/v2/users", null);
        }

        // Assert
        if (respones.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            // If the user already exists check the error message with Assert
            Assert.Contains("A user with the same GUID already exists.", await respones.Content.ReadAsStringAsync());
        }
        if (respones.StatusCode == System.Net.HttpStatusCode.Created)
        {
            
        }



    }



}
