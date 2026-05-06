using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Явное указание порта для Docker
builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services.AddDbContext<LibraryDbContext>(opt => 
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILibraryService, LibraryService>();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.EnsureCreated();
    if (!db.Books.Any())
    {
        var author = new Author { Name = "Михаил Булгаков", BirthYear = 1891 };
        db.Authors.Add(author);
        db.Books.AddRange(
            new Book { Title = "Мастер и Маргарита", Year = 1967, Author = author },
            new Book { Title = "Собачье сердце", Year = 1925, Author = author }
        );
        db.SaveChanges();
    }
}

app.MapGet("/", () => "API is running on HTTP!");
app.MapGet("/api/books", (ILibraryService service) => Results.Ok(service.GetAllBooks()));

app.Run();