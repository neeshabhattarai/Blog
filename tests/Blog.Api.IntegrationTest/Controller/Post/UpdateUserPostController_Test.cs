using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class UpdateUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> Factory { get; set; }
    public HttpClient Client{get;set;}
    public UserControllerTest UserController { get; set; }
    public UpdateUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        UserController = new UserControllerTest(factory);
        Client =factory.CreateClient();
    }
    public async Task<string> AddUserPost()
    {
        await UserController.RegisterUser();
        var tokenGen= await UserController.GenerateToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenGen);
        await UserController.AddRole(tokenGen);
        var newToken= await UserController.GenerateToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        },JsonSerializerOptions.Default);
        var id= await response.Content.ReadAsStringAsync();
        Assert.Equal(response.StatusCode,HttpStatusCode.Created);
        return id;
    }

    [Fact]
    public async Task UpdateUserPost_ShouldReturnOk()
    {
        var id=await AddUserPost();
        var response = await Client.PutAsJsonAsync($"/api/Post/UpdateUserPost/{id}", new
        {
            PostTitle = "Test Title"
        },JsonSerializerOptions.Default);
        // response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
    }
    [Fact]
    public async Task UpdateUserPost_ShouldReturnNotFound()
    {
        var id =await AddUserPost();
        
        var response = await Client.DeleteAsync("/api/Post/DeleteUserPostById/ed741b32-77d4-4a80-9367-74e441c1e6c6");
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }

}