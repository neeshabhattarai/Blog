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
    public async Task DeleteUserPost_ShouldReturnNotFound()
    {
        var getClient = client.CreateClient();
        var token = new GetUserController_Test(client);
        var tokens=await token.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        var response=await getClient.DeleteAsync($"/api/Post/DeleteUserPostById/33");
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteUserPost_ShouldReturnSuccess()
    {
        var getClient = client.CreateClient();
        var token = new GetUserController_Test(client);
        var tokens=await token.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        var response=await getClient.DeleteAsync($"/api/Post/DeleteUserPostById/065ae4ca-70c8-4a05-80ee-184d7beb4fee");
        Assert.Equal(response.StatusCode,HttpStatusCode.NoContent);
    }
}