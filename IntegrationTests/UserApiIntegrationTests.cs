using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using UserManagement;
using System.Collections.Generic;

public class UserApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task ResetDatabase()
    {
        await _client.DeleteAsync("/api/auth/reset-test-users");
    }

    [Fact]
    public async Task RegisterUser_ShouldReturn_Ok()
    {
        // Arrange: Reset the database and create a new user
        await ResetDatabase();
        var newUser = new { Name = "Test User", Email = "test@example.com", Password = "StrongPass123" };

        // Act: Attempt to register the user
        var response = await _client.PostAsJsonAsync("/api/auth/register", newUser);

        // Assert: Verify that the registration was successful
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturn_Token()
    {
        // Arrange: Reset the database and register a user
        await ResetDatabase();
        var registerUser = new { Name = "Test User", Email = "test@example.com", Password = "StrongPass123" };
        await _client.PostAsJsonAsync("/api/auth/register", registerUser);

        var loginRequest = new { Email = "test@example.com", Password = "StrongPass123" };

        // Act: Attempt to log in
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert: Verify that the login was successful and a token is returned
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var loginData = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        
        Assert.NotNull(loginData);
        Assert.True(loginData.ContainsKey("token"), "Response does not contain a token.");

        string token = loginData["token"];
        Assert.False(string.IsNullOrEmpty(token), "Token is null or empty.");
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturn_Ok()
    {
        // Arrange: Reset the database, register a user, and log in
        await ResetDatabase();
        var registerUser = new { Name = "Test User", Email = "test@example.com", Password = "StrongPass123" };
        await _client.PostAsJsonAsync("/api/auth/register", registerUser);

        var loginRequest = new { Email = "test@example.com", Password = "StrongPass123" };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Ensure login was successful
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        
        var loginData = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(loginData);
        Assert.True(loginData.ContainsKey("token"), "Response does not contain a token.");

        string token = loginData["token"];
        Assert.False(string.IsNullOrEmpty(token), "Token is null or empty.");

        // Add the token to the request headers
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act: Attempt to get the current user
        var response = await _client.GetAsync("/api/users/me");

        // Assert: Verify that the request was successful
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturn_Conflict_When_Email_Already_Exists()
    {
        // Arrange: Reset the database and register a user
        await ResetDatabase();
        var newUser = new { Name = "Test User", Email = "test@example.com", Password = "StrongPass123" };

        await _client.PostAsJsonAsync("/api/auth/register", newUser);

        // Act: Attempt to register the same user again
        var response = await _client.PostAsJsonAsync("/api/auth/register", newUser);

        // Assert: Verify that the response is Conflict (409)
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturn_Unauthorized_For_Invalid_Password()
    {
        // Arrange: Reset the database, register a user
        await ResetDatabase();
        var registerUser = new { Name = "Test User", Email = "test@example.com", Password = "StrongPass123" };
        await _client.PostAsJsonAsync("/api/auth/register", registerUser);

        var invalidLoginRequest = new { Email = "test@example.com", Password = "WrongPass123" };

        // Act: Attempt to log in with an incorrect password
        var response = await _client.PostAsJsonAsync("/api/auth/login", invalidLoginRequest);

        // Assert: Verify that the response is Unauthorized (401)
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUser_ShouldReturn_Unauthorized_Without_Token()
    {
        // Act: Attempt to get user details without a token
        var response = await _client.GetAsync("/api/users/me");

        // Assert: Verify that the response is Unauthorized (401)
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
