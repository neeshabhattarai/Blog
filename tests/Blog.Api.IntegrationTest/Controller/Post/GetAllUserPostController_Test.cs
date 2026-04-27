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
       var tokens=await message.Content.ReadFromJsonAsync<GetToken>();
        getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);
       var response=await getClient.GetAsync($"/api/Post/GetAllUserPost");
       response.EnsureSuccessStatusCode();
       Assert.Equal(response.IsSuccessStatusCode,true);
    }
    
    // [Fact]
    // public async Task GetAllUserPost_ShouldReturnNotFound()
    // {
    //     var getClient = client.CreateClient();
    //     var token = new GetUserController_Test(client);
    //     var tokens=await token.GenerateToken();
    //     getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens);
    //     var response=await getClient.GetAsync($"/api/Post/GetUserPostById/33");
    //     Assert.Equal(response.StatusCode,HttpStatusCode.NotFound);
    // }
    
}