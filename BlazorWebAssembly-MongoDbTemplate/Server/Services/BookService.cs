using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MondoDbTemplate.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace MondoDbTemplate.Server.Services {
    public class BookService : IBookService {
        private readonly IMongoCollection<Book> _books;

        public BookService(IConfiguration config) {
            var client = new MongoClient(config.GetConnectionString("BookDB"));
            var database = client.GetDatabase("BookDb");

            _books = database.GetCollection<Book>("Books");
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book) {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
