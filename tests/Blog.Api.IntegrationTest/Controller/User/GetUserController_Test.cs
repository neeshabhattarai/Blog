using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blog.Api.IntegrationTest.Token;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blog.Api.IntegrationTest.Controller.User;

public class GetUserController_Test:IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient client { get; set; }
    public GetUserController_Test(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUser_ReturnSuccess()
    {
        var token = await GenerateToken();
        
    }

    public async Task<string> GenerateToken()
    {
        var request = await client.PostAsJsonAsync("api/User/GenerateToken", new
        {
            Email = "bhattaraineesha922@gmail.com",
            Password = "String@123"
        }, JsonSerializerOptions.Default);
        request.EnsureSuccessStatusCode();
        Assert.Equal(request.StatusCode, HttpStatusCode.OK);
        var result = await request.Content.ReadFromJsonAsync<GetToken>();
        return result.Token;
    }
    
}