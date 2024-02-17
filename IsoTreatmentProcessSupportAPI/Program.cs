using FluentValidation;
using FluentValidation.AspNetCore;
using IsoTreatmentProcessSupportAPI;
using IsoTreatmentProcessSupportAPI.Entities;
using IsoTreatmentProcessSupportAPI.Middlewares;
using IsoTreatmentProcessSupportAPI.Models;
using IsoTreatmentProcessSupportAPI.Models.Validators;
using IsoTreatmentProcessSupportAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddCookie(x =>
{
    x.Cookie.Name = "token";

}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
    cfg.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<IsoSupportDbContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateEntryDto>, CreateEntryDtoValidator>();
builder.Services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
builder.Services.AddTransient<IMailkitService, MailkitService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<ITreatmentProcessService, TreatmentProcessService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", builder =>
    builder.AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins("http://localhost:5173")
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("FrontendClient");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iso Treatment Process Support API");
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
