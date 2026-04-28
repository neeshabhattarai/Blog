using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper.Configuration.Annotations;
using Blog.Api.IntegrationTest.Services;
using Blog.Api.IntegrationTest.TestHelpers;
using Blog.Api.IntegrationTest.Token;
using Blog.Application.User.DTO;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace Blog.Api.IntegrationTest.Controller.User;

public class UserControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly UserService _userService;
    private AuthHelpers _authHelpers;
    public UserControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client=factory.CreateClient();
        _userService=new UserService(_factory);
        _authHelpers=new AuthHelpers(_factory);
        
        
    }

    public async Task<HttpResponseMessage> RegisterUser()
    {
        var response = await _userService.AddUser();
        return response;
    }

    public async Task<HttpResponseMessage> AddRole()
    {
        var token=await _authHelpers.GenerateToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await _client.PostAsJsonAsync<object>("/api/User/AddRole?roleName=User",_userService.GetUser());
        return response;
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
         _userService._addUser.Password = "123456";
        var response=await _client.PostAsJsonAsync("/api/User/RegisterUser",_userService.GetUser());
        Assert.Equal(response.StatusCode,HttpStatusCode.BadRequest);
        Assert.NotNull(response.Content);
    }

    [Fact]
    public async Task LoginWith2Fa_ShouldSendMessage()
    {
        
        var token=await _authHelpers.GenerateToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await _client.PostAsJsonAsync("/api/User/LoginWith2FA",await _userService.GetUser());
        _factory.EmailService.Verify(x=>x.SendEmailAsync("test@gmail.com",It.IsAny<string>(),It.IsAny<string>()),Times.Once);
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        Assert.NotNull(response.Content);
    }
    [Fact]
    public async Task AddRole_WithInvalidToken_ShouldReturnUnauthorized()
    {
        var message=await _authHelpers.GenerateToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",message+"a");
        var roleAssigned=await _client.PostAsync("/api/User/AddRole?roleName=Admin",null);
        Assert.NotNull(message);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.Unauthorized);
        ;
    }

    [Fact]
    public async Task AddRole_WithOutToken_ShouldReturnUnauthorized()
    {
        
        var roleAssigned=await _client.PostAsync("/api/User/AddRole?roleName=Admin",null);
        Assert.Equal(roleAssigned.StatusCode,HttpStatusCode.Unauthorized);
        ;
    }
    [Fact]
    public async Task GenerateToken_ShouldReturnOk()
    {
        var response=await _authHelpers.GenerateToken();
        Assert.NotNull(response);
    }

    
    [Fact]
    public async Task AddRole_ShouldReturnOk()
    {
        var response = await AddRole();
       Assert.NotNull(response);
        
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
    [Fact]
    public async Task AddRole_ShouldReturnBadRequest()
    {
        var response=await _authHelpers.GenerateToken();
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
        var responseMessage = await _authHelpers.GenerateToken();
        Assert.NotNull(responseMessage);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",responseMessage);
        
        var response = await _client.PostAsJsonAsync("/api/User/LoginWith2FA",await _userService.GetUser());
        response.EnsureSuccessStatusCode();
        Assert.Equal(response.StatusCode,HttpStatusCode.OK);
        var configure=await _client.GetAsync($"/api/User/Configure2FA?token={otp}");
       Assert.NotNull(configure);
       Assert.Equal(configure.StatusCode,HttpStatusCode.OK);
        

    }
    
}
