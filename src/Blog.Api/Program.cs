using Blog.Application.Extensions;
using Blog.Infastructure.Extensions;
using Blog.Services;
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
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

    };
});
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<SMTPConfigure>(builder.Configuration.GetSection("SMTP"));
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
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
app.UseAuthentication();
app.MapControllers();
app.Run();

