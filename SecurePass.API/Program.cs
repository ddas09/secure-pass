using Hangfire;
using System.Text;
using SecurePass.Data;
using System.Reflection;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Models;
using SecurePass.API.Extensions;
using SecurePass.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecurePass.Services.Extensions;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// This is to configure custom ModelState validation filter
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SecurePass API", Version = "v1" });

    // To include XML comments in Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// For generating lowecase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Adding DB context
builder.Services.AddDbContext<SecurePassDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SecurePass"));
});

// Binding custom models with configurations
builder.Services.Configure<JwtConfiguration>
    (
        builder.Configuration.GetSection(nameof(JwtConfiguration))
    );

builder.Services.Configure<MailConfiguration>
    (
        builder.Configuration.GetSection(nameof(MailConfiguration))
    );

// For registering services for DI
builder.Services.RegisterServices();

// For registering repositories for DI
builder.Services.RegisterRepositories();

// For registering action filters
builder.Services.RegisterActionFilters();

// For AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Hanfire Configurations
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("SecurePass")));

builder.Services.AddHangfireServer();

// For configuring CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// For configuring JWT Authentication
var jwtConfiguration = builder.Configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.AccessTokenSecret)),
        ValidIssuer = jwtConfiguration.Issuer,
        ValidAudience = jwtConfiguration.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions
    {
        Authorization = [ new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            RequireSsl = false,
            SslRedirect = false,
            LoginCaseSensitive = true,
            Users =
            [
                new BasicAuthAuthorizationUser
                {
                    Login = builder.Configuration.GetSection(nameof(Hangfire)).GetValue(typeof(string), "AdminUser").ToString(),
                    PasswordClear =  builder.Configuration.GetSection(nameof(Hangfire)).GetValue(typeof(string), "BasicAuthPass").ToString()
                },
            ]
        })],
        IgnoreAntiforgeryToken = true
    });
}

// For configuring custom middlewares
app.ConfigureMiddlewares();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
