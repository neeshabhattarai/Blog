using System.Text;
using Blog.Application.Extensions;
using Blog.Infastructure.Extensions;
using Blog.Middleware;
using Blog.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationService();

builder.Services.AddControllersWithViews();
builder.Services.AddInfastructureServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ExceptionHandler>();
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
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.HttpContext.Request.Cookies["access_token"];
            if (!String.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            Console.WriteLine(token);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine(context.Exception.Message);
            return Task.CompletedTask;
        }
    };
    
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAntiforgery(options =>
{
options.HeaderName = "X-CSRF-TOKEN";
options.SuppressXFrameOptionsHeader = false;
});
builder.Services.Configure<SMTPConfigure>(builder.Configuration.GetSection("SMTP"));
builder.Services.AddSwaggerGen(options =>
{
     options.AddSecurityDefinition("access_token",new OpenApiSecurityScheme()
     {
         Type = SecuritySchemeType.ApiKey,
         In = ParameterLocation.Cookie,
         Name = "access_token",
     });
    options.AddSecurityDefinition("X-CSRF-TOKEN",new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In=ParameterLocation.Header,
        Name = "X-CSRF-TOKEN",
    });
    // options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    // {
    //     {new OpenApiSecurityScheme
    //     {
    //         Reference = new OpenApiReference
    //         {
    //             Type = ReferenceType.SecurityScheme,
    //             Id = "BearerScheme"
    //         }
    //     },
    //     new List<string>()}
    // });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-CSRF-TOKEN"
                }
            },
            new List<string>()
        },
        
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "access_token"
                }
            },
            new List<string>()
        },

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
// app.UseAntiforgery();
app.MapGet("/api/csrf", (IAntiforgery antiforgery, HttpContext context) =>
{
   var token= antiforgery.GetAndStoreTokens(context);
   return Results.Ok(token.RequestToken);
});
app.MapControllers();
app.Run();

public partial class Program
{
    
}

