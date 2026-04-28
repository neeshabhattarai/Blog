using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class DeleteUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> factory { get; set; }
    
    public AuthHelpers _authHelpers { get; set; }
    public HttpClient Client { get; set; }
    public DeleteUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        this.factory=factory;
        _authHelpers=new (factory);
        Client = factory.CreateClient();

    }
    public async Task<string> AddUserPost()
    {
        var newToken = await _authHelpers.GenerateTokenWithRole("User");
        Assert.NotNull(newToken);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        });
        var content= await response.Content.ReadFromJsonAsync<AddUserPostResponse>();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return content.PostId;
    }
    [Fact]
    public async Task DeleteUserPost_ShouldReturnNotFound()
    {
        var id=await AddUserPost();
        var response=await Client.DeleteAsync($"/api/Post/DeleteUserPostById/33");
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteUserPost_ShouldReturnSuccess()
    {
        
        var id=await AddUserPost();
        var response=await Client.DeleteAsync($"/api/Post/DeleteUserPostById/{id}");
        Assert.Equal(response.StatusCode,HttpStatusCode.NoContent);
    }
}