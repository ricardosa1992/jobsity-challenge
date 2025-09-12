using JobsityChallenge.WebApi.Extensions;

AppContext.SetSwitch("SqlClient.DisableInsecureTLSWarning", true);
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddIdentity();

var app = builder.Build();

app.ApplyDatabaseMigrations();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
