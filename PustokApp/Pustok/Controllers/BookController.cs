using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using System.Linq;

namespace Pustok.Controllers
{
    public class BookController:Controller
    {
        private readonly PustokDbContext _context;

        public BookController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult GetBook(int id)
        {
            Book book = _context.Books.Include(x=> x.Genre).Include(x=> x.BookImages).Include(x=> x.Author).FirstOrDefault(x=> x.Id == id);
            return PartialView("_BookModalPartial", book );
        }
    }
}
