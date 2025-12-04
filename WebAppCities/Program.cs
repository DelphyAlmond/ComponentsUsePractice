
using Microsoft.EntityFrameworkCore;
using WebAppCities.Models;
using WebAppCities;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// >> API Endpoints

// GET /cities
app.MapGet("/cities", async (AppDbContext db) =>
{
    return await db.Cities.ToListAsync();
});

// PUT /cities Ч добавить
app.MapPut("/cities", async (AppDbContext db, City city) =>
{
    if (string.IsNullOrWhiteSpace(city.Name)) return Results.BadRequest("Name is required");
    db.Cities.Add(city);
    await db.SaveChangesAsync();
    return Results.Ok(city);
});

// POST /cities/{name} Ч обновить
app.MapPost("/cities/{name}", async (AppDbContext db, string name, City updated) =>
{
    var existing = await db.Cities.FirstOrDefaultAsync(c => c.Name == name);
    if (existing == null) return Results.NotFound();
    existing.Name = updated.Name;
    await db.SaveChangesAsync();
    return Results.Ok(existing);
});

// DELETE /cities/{name}
app.MapDelete("/cities/{name}", async (AppDbContext db, string name) =>
{
    var city = await db.Cities.FirstOrDefaultAsync(c => c.Name == name);
    if (city == null) return Results.NotFound();
    db.Cities.Remove(city);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();