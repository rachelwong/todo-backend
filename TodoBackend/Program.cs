using Microsoft.EntityFrameworkCore;
using TodoBackend.models;

var builder = WebApplication.CreateBuilder(args);

string[] safeUrls = ["http://localhost:4200"];

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("Todos")); // registers in memory db

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins(safeUrls)
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // displays error page
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    if (!db.TodoItems.Any())
    {
        db.TodoItems.AddRange(new TodoItem {
            Id = Guid.NewGuid().ToString(),
            Description = "Running",
            Done = false,
        }, new TodoItem
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Cleaning",
            Done = true,
        });
        db.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.UseCors("AllowAll"); 
app.Run();
