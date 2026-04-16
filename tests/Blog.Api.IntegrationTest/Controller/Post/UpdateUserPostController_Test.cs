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
        var response = await client.PutAsJsonAsync("/api/Post/UpdateUserPost/1a7ff143-fd56-46ac-80d8-d28aedb227b0", new
        {
            PostTitle = "Test Title"
        },JsonSerializerOptions.Default);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
    }
    [Fact]
    public async Task UpdateUserPost_ShouldReturnNotFound()
    {
        var client = Factory.CreateClient();
        var tokens =await new GetUserController_Test(Factory).GenerateToken();
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", tokens);
        var response = await client.DeleteAsync("/api/Post/DeleteUserPostById/76316a1f-b911-4ce0-928d-8593461187a8");
        // response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }

}