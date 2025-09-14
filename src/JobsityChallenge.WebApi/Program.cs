using JobsityChallenge.Core.Interfaces.Repositories;
using JobsityChallenge.Core.Interfaces.Services;
using JobsityChallenge.Core.Services;
using JobsityChallenge.Infrastructure.Data.Repositories;
using JobsityChallenge.Shared.Hubs;
using JobsityChallenge.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddIdentity();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddSignalR();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5002")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");
app.ApplyDatabaseMigrations();

app.Run();
