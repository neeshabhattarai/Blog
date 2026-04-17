using System.Text;
using Blog.Application.Extensions;
using Blog.Infastructure.Extensions;
using Blog.Middleware;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationService();

builder.Services.AddControllers();
builder.Services.AddInfastructureServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<SMTPConfigure>(builder.Configuration.GetSection("SMTP"));
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("BearerScheme",new OpenApiSecurityScheme()
    {
        BearerFormat = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "BearerScheme"
            }
        },
        new List<string>()}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program
{
    
}

