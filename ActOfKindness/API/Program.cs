using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Extensions;
using Application.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Persistence;
using Persistence.Extensions;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEventDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policy =>
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins(builder.Configuration["AllowedOrigins"]).WithOrigins("https://localhost:3000"));
});
builder.Services.AddSignalR();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc =>
    {
        lc.Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Information);
        lc.WriteTo.File("Logs/Info/info_.txt", rollingInterval: RollingInterval.Day);
    })
    .WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Warning();
        lc.WriteTo.File("Logs/Errors/error_.txt", rollingInterval: RollingInterval.Day);
    })
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();

    try
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
        await seeder.SeedAsync();
    }
    catch (Exception exception)
    {
        Log.Fatal($"Error while creating database | {exception}");
    }
}

app.UseHttpsRedirection();
app.UseCors("FrontEndClient");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
