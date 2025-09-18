using UserManagementAPI.Services;
using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Configure middleware pipeline
app.UseMiddleware<ErrorHandlingMiddleware>();      // Handles exceptions first
app.UseMiddleware<AuthenticationMiddleware>();     // Validates tokens
app.UseMiddleware<LoggingMiddleware>();            // Logs requests/responses

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthorization(); // Optional, if using [Authorize] attributes

app.MapControllers();   // Maps controller endpoints

app.Run();