using MondoDbTemplate.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MondoDbTemplate.Client.ViewModels {
    public class BookViewModel : IBookViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Message { get; set; }

        private HttpClient _httpClient;

        public BookViewModel() {

        }

        public BookViewModel(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task GetBook() {
            Book book = await _httpClient.GetFromJsonAsync<Book>("Book/getbook/5f1893f82346dad59fca9f8b");
            Id = book.Id;
            Name = book.Name;
            Price = book.Price;

            Message = "Loaded Book";
        }

        public async Task UpdateBook() {
            Book oldBook = this;
            await _httpClient.PutAsJsonAsync("Book/updatebook/5f1893f82346dad59fca9f8b", oldBook);
            Message = "Updated Book";
        }

        public static implicit operator BookViewModel(Book book) {
            return new BookViewModel {
                Id = book.Id,
                Name = book.Name,
                Price = book.Price
            };
        }

        public static implicit operator Book(BookViewModel bookViewModel) {
            return new Book {
                Id = bookViewModel.Id,
                Name = bookViewModel.Name,
                Price = bookViewModel.Price
            };
        }
    }
}
