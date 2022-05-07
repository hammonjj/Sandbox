using System.Threading.Tasks;

namespace MondoDbTemplate.Client.ViewModels {
    public interface IBookViewModel {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Task GetBook();
        public Task UpdateBook();
    }
}