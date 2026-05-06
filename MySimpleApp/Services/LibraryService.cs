using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class LibraryService : ILibraryService
{
    private readonly LibraryDbContext _db;
    public LibraryService(LibraryDbContext db) => _db = db;

    public List<Book> GetAllBooks() => _db.Books.Include(b => b.Author).ToList();
    public string GetStatus() => $"Книг в системе: {_db.Books.Count()}";
}