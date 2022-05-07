using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MondoDbTemplate.Server.Services;
using MondoDbTemplate.Shared.Models;
using System.Threading.Tasks;

namespace MondoDbTemplate.Server.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class BookController : Controller {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) {
            _bookService = bookService;
        }

        [HttpGet]
        public Book Get() {
            return _bookService.Get("5f1893f82346dad59fca9f8b");
        }

        [HttpGet("getbook/{bookId}")]
        public Book Get(string bookId) {
            return _bookService.Get(bookId);
        }

        [HttpPut("updatebook/{bookId}")]
        public void UpdateBook(string bookId, [FromBody] Book book) {
            _bookService.Update(bookId, book);
        }

        /* Default Controller Actions */
#if false
        // GET: BookController
        public ActionResult Index() {
            return View(_bookService.Get());
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: BookController/Create
        public ActionResult Create(Book book) {
            if(ModelState.IsValid) {
                _bookService.Create(book);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(string id, Book book) {
            if(id != book.Id) {
                return NotFound();
            }
            if(ModelState.IsValid) {
                _bookService.Update(id, book);
                return RedirectToAction(nameof(Index));
            } else {
                return View(book);
            }
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(string id) {
            if(id == null) {
                return NotFound();
            }

            var book = _bookService.Get(id);
            if(book == null) {
                return NotFound();
            }
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
#endif
    }
}
