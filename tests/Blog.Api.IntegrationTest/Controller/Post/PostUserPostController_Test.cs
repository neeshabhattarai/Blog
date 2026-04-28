using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.TestHelpers;
using Blog.Controller;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class PostUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> _factory { get; set; }
    public HttpClient Client { get; set; }
    public AuthHelpers _authHelpers { get; set; }
    public PostUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        Client = _factory.CreateClient();
        _authHelpers = new AuthHelpers(_factory);
        
    }


    [Fact]
    public async Task PostUserPost_ShouldReturnCreatedAction()
    {
        var token=await _authHelpers.GenerateTokenWithRole("User");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        });
        var content=await response.Content.ReadFromJsonAsync<AddUserPostResponse>();
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created,response.StatusCode);
        Assert.NotNull(content);
        Assert.NotEqual(Guid.Empty.ToString(), content.PostId);
        var result = await Client.GetAsync($"api/Post/GetUserPostById/{content.PostId}");
        var contentString = await result.Content.ReadFromJsonAsync<AddUserPostResponse>();
        Assert.Equal(HttpStatusCode.OK,result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.Equal(content.PostTitle,contentString.PostTitle);
        
    }
    [Fact]
    public async Task PostUserPost_WithEmptyBody_ReturnBadRequest()
    {
       var token= await _authHelpers.GenerateTokenWithRole("User");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        });
       Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task PostUserPost_WithoutToken_ReturnUnauthorized()
    {
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        });
        Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
    }
    
    [Fact]
    public async Task PostUserPost_WithoutRole_ReturnForbidden()
    {
       var token= await _authHelpers.GenerateToken();
       Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    [Fact]
    public async Task PostUserPost_WithInvalidToken_ReturnUnauthorized()
    {
        var token = await _authHelpers.GenerateTokenWithRole("User");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token+'a');
        var response = await Client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
}