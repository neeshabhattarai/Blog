using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.TestHelpers;
using Blog.Api.IntegrationTest.Token;
using Blog.Application.UserPost.DTO;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Blog.Api.IntegrationTest.Controller;

public class GetAllUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> factory { get; set; }
    public HttpClient _client { get; set; }
    private readonly AuthHelpers _authHelpers;
    
    public GetAllUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        this.factory=factory;
        _client=factory.CreateClient();
        _authHelpers=new AuthHelpers(factory);
    }
    
    [Fact]
    public async Task GetAllUserPost_ShouldReturnOk()
    {
     var token= await _authHelpers.GenerateTokenWithRole("User");
     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
       var response=await _client.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=1&pageSize=5&orderBy=PostTitle&sortDirection=desc");
       var result=await response.Content.ReadFromJsonAsync<PageResult<ReadUserPostDTO>>();
       Assert.NotNull(result);
       Assert.True(result.list.Count>=0);
       response.EnsureSuccessStatusCode();
       Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllUserPost_WithInvalidPagination_ShouldReturnBadRequest()
    {
        var token= await _authHelpers.GenerateTokenWithRole("User");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response=await _client.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=-2&pageSize=-5&orderBy=PostTitle&sortDirection=desc");
        
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task GetAllUserPost_ShouldReturnUnauthorized()
    {
        var response=await _client.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=1&pageSize=5&orderBy=PostTitle&sortDirection=desc");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    [Fact]
    public async Task GetAllUserPosts_ShouldReturnForbidden()
    {
        var token= await _authHelpers.GenerateToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response=await _client.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=1&pageSize=5&orderBy=PostTitle&sortDirection=desc");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
}