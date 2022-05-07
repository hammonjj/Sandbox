using System.Collections.Generic;
using MondoDbTemplate.Shared.Models;

namespace MondoDbTemplate.Server.Services {
    public interface IBookService {
        List<Book> Get();

        Book Get(string id);

        Book Create(Book book);

        void Update(string id, Book bookIn);

        void Remove(Book bookIn);

        void Remove(string id);
    }
}
