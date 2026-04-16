using System.Net;
using System.Net.Http.Headers;
using Blog.Api.IntegrationTest.Controller.User;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class DeleteUserPostController_Test:IClassFixture<WebApplicationFactory<Program>>
{
    public WebApplicationFactory<Program> client { get; set; }
    public DeleteUserPostController_Test(WebApplicationFactory<Program> factory)
    {
        client = factory;
    }
    [Fact]
    public async Task GetAllUserPost_ShouldReturnNotFound()
    {
        var getClient = client.CreateClient();
        var token = new GetUserController_Test(client);
        var tokens=await token.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        var response=await getClient.DeleteAsync($"/api/Post/DeleteUserPostById/33");
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAllUserPost_ShouldReturnSuccess()
    {
        var getClient = client.CreateClient();
        var token = new GetUserController_Test(client);
        var tokens=await token.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        var response=await getClient.DeleteAsync($"/api/Post/DeleteUserPostById/1a7ff143-fd56-46ac-80d8-d28aedb227b0");
        Assert.Equal(response.StatusCode,HttpStatusCode.NoContent);
    }
}