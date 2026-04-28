using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.Token;
using Blog.Application.User.DTO;

namespace Blog.Api.IntegrationTest.TestHelpers;

public class AuthHelpers
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly UserService _userService;
    private readonly HttpClient _client;

    public AuthHelpers(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _userService = new UserService(_factory);
        _client = _factory.CreateClient();
    }

    public async Task<string> GenerateTokenWithRole(string role)
    {
        await GenerateToken();
        await _client.PostAsync($"/api/User/AddRole?roleName={role}", null);
       var newResponse= await _client.PostAsJsonAsync<object>("/api/User/GenerateToken", await  _userService.GetUser());
       var newToken=await newResponse.Content.ReadFromJsonAsync<GetToken>(); 
       Assert.NotNull(newToken);
       _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken.Token);
        return newToken.Token;
    }

    public async Task<string> GenerateToken()
    {
        await _userService.AddUser();
        var response=await _client.PostAsJsonAsync("/api/User/GenerateToken",await  _userService.GetUser());
        var readResponse =await response.Content.ReadFromJsonAsync<GetToken>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", readResponse.Token);
        return readResponse.Token;
    }
}