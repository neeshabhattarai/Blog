using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.TestHelpers;
using Blog.Application.UserPost.Command.UpdateUserPost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller;

public class UpdateUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> _factory { get; set; }
    public HttpClient _client{get;set;}
    public AuthHelpers _authHelpers { get; set; }
    public UpdateUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        _factory=factory;
        _client=factory.CreateClient();
        _authHelpers = new(factory);
    }
    public async Task<string> AddUserPost()
    {
        var token = await _authHelpers.GenerateTokenWithRole("User");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await _client.PostAsJsonAsync("/api/Post/AddUserPost", new
        {
            postTitle = "Test Title"
        },JsonSerializerOptions.Default);
        var content= await response.Content.ReadFromJsonAsync<AddUserPostResponse>();
        Assert.Equal(response.StatusCode,HttpStatusCode.Created);
        return content.PostId;
    }

    [Fact]
    public async Task UpdateUserPost_ShouldReturnOk()
    {
        var id=await AddUserPost();
        var response = await _client.PutAsJsonAsync($"/api/Post/UpdateUserPost/{id}", new
        {
            PostTitle = "Test Title"
        },JsonSerializerOptions.Default);
        response.EnsureSuccessStatusCode();
        var content=await response.Content.ReadFromJsonAsync<AddUserPostResponse>();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        Assert.Equal(content.PostId,id);
        Assert.Equal(content.PostTitle,"Test Title");
    }
    [Fact]
    public async Task UpdateUserPost_ShouldReturnBadRequest()
    {
        var id=await AddUserPost();
        var response = await _client.PutAsJsonAsync($"/api/Post/UpdateUserPost/{id}",new UpdateUserPostCommand
        {
            PostTitle = ""
        });
        Assert.Equal(response.StatusCode,HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task UpdateUserPost_ShouldReturnNotFound()
    {
        var id=await AddUserPost();
        var response = await _client.PutAsJsonAsync($"/api/Post/UpdateUserPost/{id+"ad"}",new UpdateUserPostCommand
        {
            PostTitle = "Test"
        });
        Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    }


}