using UserManagementAPI.Models;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", (UserService service) => service.GetAll());

app.MapGet("/users/{id}", (int id, UserService service) =>
    service.GetById(id) is User user ? Results.Ok(user) : Results.NotFound());

app.MapPost("/users", (User user, UserService service) =>
{
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
