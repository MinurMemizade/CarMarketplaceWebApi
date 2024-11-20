using CarMarketplaceWebApi.Context;
using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Repositories.Implementations;
using CarMarketplaceWebApi.Repositories.Interfaces;
using CarMarketplaceWebApi.Services.Implementations;
using CarMarketplaceWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarMarketplaceWebApi.Models.Security;
using Serilog;
using CarMarketplaceWebApi.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) // Read settings from appsettings.json
    .ReadFrom.Services(services) // Optionally add logging for services
);

// Add services to the container.
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")).EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Logs to the console
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs to a file, with daily rotation
    .CreateLogger();



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64; 
    });

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "App.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token after writin 'Bearer'"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRevokeService, RevokeService>();
builder.Services.AddScoped<IFileManagerService, FileManagerService>();
builder.Services.AddScoped<TokenService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("JWT")); //bu hissesin yadda saxla lazim olsa DI ile et
builder.Services.AddTransient<ITokenSevice,TokenService>(); //hemcinin bunu da
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options=>
{
    options.SaveToken = true;
    options.TokenValidationParameters=new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),// Secret deyil Key 
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddFluentEmail(
    builder.Configuration["Email:SenderEmail"],
    builder.Configuration["Email:Sender"]).AddSmtpSender(
    builder.Configuration["Email:Host"], builder.Configuration.GetValue<int>("Email:Port"));


builder.Services.AddIdentityCore<AppUser>(opt=>
opt.SignIn.RequireConfirmedAccount=true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
