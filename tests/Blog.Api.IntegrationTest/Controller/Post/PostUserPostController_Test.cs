using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class PostUserPostController_Test:IClassFixture<WebApplicationFactory<Program>>
{
    public WebApplicationFactory<Program> Factory { get; set; }
    public PostUserPostController_Test(WebApplicationFactory<Program> factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task PostUserPost_ShouldReturnCreatedAction()
    {
        var client = Factory.CreateClient();
        var tokens =await new GetUserController_Test(Factory).GenerateToken();
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", tokens);
        var response = await client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        },JsonSerializerOptions.Default);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.Created);
    }
    [Fact]
    public async Task PostUserPost_ShouldReturnBadRequest()
    {
        var client = Factory.CreateClient();
        var tokens =await new GetUserController_Test(Factory).GenerateToken();
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", tokens);
        var response = await client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        },JsonSerializerOptions.Default);
       Assert.Equal(response.StatusCode,HttpStatusCode.BadRequest);
    }
}