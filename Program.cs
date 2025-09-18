using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Middleware;
using UserManagementAPI.Models;
using UserManagementAPI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Order matters: Error → Auth → Logging
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

// Swagger and endpoints
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", (UserService service, int? skip, int? take) =>
{
    var users = service.GetAll();
    if (skip.HasValue || take.HasValue)
    {
        users = users.Skip(skip ?? 0).Take(take ?? users.Count).ToList();
    }
    return Results.Ok(users);
});

app.MapGet("/users/{id}", (int id, UserService service) =>
{
    try
    {
        var user = service.GetById(id);
        return user is not null ? Results.Ok(user) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }
});


app.MapPost("/users", (User user, UserService service) =>
{
    var validationContext = new ValidationContext(user);
    var results = new List<ValidationResult>();
    if (!Validator.TryValidateObject(user, validationContext, results, true))
    {
        return Results.BadRequest(results);
    }

    service.Add(user);
    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", (int id, User updatedUser, UserService service) =>
{
    updatedUser.Id = id;
    return service.Update(updatedUser) ? Results.Ok(updatedUser) : Results.NotFound();
});

app.MapDelete("/users/{id}", (int id, UserService service) =>
    service.Delete(id) ? Results.Ok() : Results.NotFound());

app.Run();
