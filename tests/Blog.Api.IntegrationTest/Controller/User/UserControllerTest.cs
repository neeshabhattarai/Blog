using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper.Configuration.Annotations;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.Token;
using Blog.Application.User.DTO;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace Blog.Api.IntegrationTest.Controller.User;

public class UserControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly AddUser _addUser;
    public UserControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client=factory.CreateClient();
        _addUser = new AddUser
        {
            Email = "test@gmail.com",
            Password = "Test@123"
        };
    }

    public async Task<HttpResponseMessage> RegisterUser()
    {
        var response = await _client.PostAsJsonAsync("/api/User/RegisterUser",_addUser);
        return response;
    }

    public async Task<HttpResponseMessage> AddRole(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await _client.PostAsJsonAsync<object>("/api/User/AddRole?roleName=User",null);
        return response;
    }

    public async Task<string> GenerateToken()
    {
        var response = await _client.PostAsJsonAsync("/api/User/GenerateToken", _addUser);
        var tokenRead=await response.Content.ReadFromJsonAsync<GetToken>();
        return tokenRead.Token;
    }

    [Fact]
    public async Task RegisterUser_WithValidValues_ShouldReturnOk()
    {
        var response = await RegisterUser();
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        Assert.NotNull(response.Content);
    }
    
    [Fact]
    public async Task RegisterUser_WithInvalidValues_ShouldReturnOk()
    {
        _addUser.Password = "123456";
        var response=await _client.PostAsJsonAsync("/api/User/RegisterUser",_addUser);
        Assert.Equal(response.StatusCode,HttpStatusCode.BadRequest);
        Assert.NotNull(response.Content);
    }

    [Fact]
    public async Task LoginWith2Fa_ShouldSendMessage()
    {
        await RegisterUser();
        var response = await _client.PostAsJsonAsync("/api/User/LoginWith2FA", _addUser);
        _factory.EmailService.Verify(x=>x.SendEmailAsync("test@gmail.com",It.IsAny<string>(),It.IsAny<string>()),Times.Once);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        Assert.NotNull(response.Content);
    }
    [Fact]
    public async Task AddRole_WithInvalidToken_ShouldReturnUnauthorized()
    {
        await RegisterUser();
        var message = await GenerateToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",message+"a");
        var roleAssigned=await _client.PostAsync("/api/User/AddRole?roleName=Admin",null);
        Assert.NotNull(message);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.Unauthorized);
        ;
    }

    [Fact]
    public async Task AddRole_WithOutToken_ShouldReturnUnauthorized()
    {
        await RegisterUser();
        var response = await GenerateToken();
        var roleAssigned=await _client.PostAsync("/api/User/AddRole?roleName=Admin",null);
        Assert.NotNull(response);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.Unauthorized);
        ;
    }
    [Fact]
    public async Task GenerateToken_ShouldReturnOk()
    {
        await RegisterUser();
        var response = await GenerateToken();
        Assert.NotNull(response);
    }

    
    [Fact]
    public async Task AddRole_ShouldReturnOk()
    {
        await RegisterUser();
        var response = await GenerateToken();
        
        var roleAssigned = await AddRole(response);
       Assert.NotNull(response);
        Assert.NotNull(roleAssigned);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.OK);
    }
    [Fact]
    public async Task AddRole_ShouldReturnBadRequest()
    {
        await RegisterUser();
        var response = await GenerateToken();
        
        Assert.NotNull(response);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",response);
        var roleAssigned=await _client.PostAsync("/api/User/AddRole?roleName=Employee",null);
        Assert.NotNull(roleAssigned);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Configure2FA_ShouldReturnOk()
    {

        var otp=default(string);
        _factory.EmailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string, string>((
                (s, s1, otpC) =>
                {
                    otp= otpC;
                }));
        await RegisterUser();
        var responseMessage = await GenerateToken();
        Assert.NotNull(responseMessage);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",responseMessage);
        
        var response = await _client.PostAsJsonAsync("/api/User/LoginWith2FA", _addUser);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        var configure=await _client.GetAsync($"/api/User/Configure2FA?token={otp}");
       Assert.NotNull(configure);
       Assert.Equal(configure.StatusCode,HttpStatusCode.OK);
        

    }
    
}
