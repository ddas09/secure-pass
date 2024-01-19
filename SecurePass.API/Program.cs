using SecurePass.Data;
using Microsoft.AspNetCore.Mvc;
using SecurePass.Common.Models;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();

// For generating lowecase routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<SecurePassDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecurePass"));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
