using IsoTreatmentProcessSupportAPI;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Middlewares;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Identity;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddControllers();
builder.Services.AddDbContext<IsoSupportDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddTransient<IMailkitService, MailkitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
