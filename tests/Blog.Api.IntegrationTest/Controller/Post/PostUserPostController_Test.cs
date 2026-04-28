using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Controller;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class PostUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> Factory { get; set; }
    public HttpClient Client { get; set; }
    public UserControllerTest UserController { get; set; }
    public PostUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        UserController = new UserControllerTest(Factory);
    }

    public async Task GetTokenWithRole()
    {
        await UserController.RegisterUser();
        var tokenGen= await UserController.GenerateToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenGen);
        await UserController.AddRole(tokenGen);
      var newToken= await UserController.GenerateToken();
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
    }

    [Fact]
    public async Task PostUserPost_ShouldReturnCreatedAction()
    {
        await GetTokenWithRole();
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        },JsonSerializerOptions.Default);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.Created);
    }
    [Fact]
    public async Task PostUserPost_ShouldReturnBadRequest()
    {
        await GetTokenWithRole();
         var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        },JsonSerializerOptions.Default);
       Assert.Equal(response.StatusCode,HttpStatusCode.BadRequest);
    }
}