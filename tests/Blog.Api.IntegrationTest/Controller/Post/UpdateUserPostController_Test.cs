using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class UpdateUserPostController_Test:IClassFixture<WebApplicationFactory<Program>>
{
    public WebApplicationFactory<Program> Factory { get; set; }
    public UpdateUserPostController_Test(WebApplicationFactory<Program> factory)
    {
        Factory = factory;
    }

    [Fact]
    public async Task UpdateUserPost_ShouldReturnOk()
    {
        var client = Factory.CreateClient();
        var tokens =await new GetUserController_Test(Factory).GenerateToken();
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", tokens);
        var response = await client.PutAsJsonAsync("/api/Post/UpdateUserPost/065ae4ca-70c8-4a05-80ee-184d7beb4fee", new
        {
            PostTitle = "Test Title"
        },JsonSerializerOptions.Default);
        // response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
    }
    [Fact]
    public async Task UpdateUserPost_ShouldReturnNotFound()
    {
        var client = Factory.CreateClient();
        var tokens =await new GetUserController_Test(Factory).GenerateToken();
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", tokens);
        var response = await client.DeleteAsync("/api/Post/DeleteUserPostById/ed741b32-77d4-4a80-9367-74e441c1e6c6");
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }

}