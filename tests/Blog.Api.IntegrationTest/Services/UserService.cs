using System.Net.Http.Json;
using Blog.Application.User.DTO;

namespace Blog.Api.IntegrationTest.Services;

public class UserService
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    public readonly AddUser _addUser;
    private readonly HttpClient _client;

    public UserService(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _addUser = new AddUser
        {
            Email = "test@gmail.com",
            Password = "Test123!",
        };
    }

    public async Task<HttpResponseMessage> AddUser()
    {
      var response=await _client.PostAsJsonAsync("/api/User/RegisterUser",_addUser);
      return response;
    }

    public async Task<AddUser> GetUser()
    {
        return _addUser;
    }
}