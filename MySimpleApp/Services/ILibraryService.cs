using LibraryApi.Models;

namespace LibraryApi.Services;

public interface ILibraryService
{
    List<Book> GetAllBooks();
    string GetStatus();
}

