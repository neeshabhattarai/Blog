using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.Api.IntegrationTest.Controller.User;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.Token;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Blog.Api.IntegrationTest.Controller;

public class GetAllUserPostController_Test:IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CustomWebApplicationFactory<Program> client { get; set; }
    private UserControllerTest userControllerTest{get;set;}
    public GetAllUserPostController_Test(CustomWebApplicationFactory<Program> factory)
    {
        client = factory;
        userControllerTest = new UserControllerTest(client);
    }

    [Fact]
    public async Task GetAllUserPost_ShouldReturnOk()
    {
        var getClient = client.CreateClient();
        var token =await userControllerTest.RegisterUser();
        var message = await userControllerTest.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", message);
        await userControllerTest.AddRole(message);
        var newToken = await userControllerTest.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",newToken);
       var response=await getClient.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=1&pageSize=5&orderBy=PostTitle&sortDirection=desc");
       response.EnsureSuccessStatusCode();
       Assert.Equal(response.IsSuccessStatusCode,true);
    }
    
    [Fact]
    public async Task GetAllUserPost_ShouldReturnNotFound()
    {
        var getClient = client.CreateClient();
        var token =await userControllerTest.RegisterUser();
        var tokens = await userControllerTest.GenerateToken();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
        var response=await getClient.GetAsync("/api/Post/GetAllUserPosts?searchText=hello&pageIndex=1&pageSize=5&orderBy=PostTitle&sortDirection=desc");
        
        Assert.Equal(response.StatusCode,HttpStatusCode.Forbidden);
    }
    
}