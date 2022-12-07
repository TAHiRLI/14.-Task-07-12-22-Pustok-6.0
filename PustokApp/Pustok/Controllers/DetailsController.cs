using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Controllers
{
    public class DetailsController:Controller
    {
        private readonly PustokDbContext _context;

        public DetailsController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id )
        {
            Book Book = _context.Books.Include(x=> x.Author).Include(x=> x.BookImages).Include(x=> x.Genre).FirstOrDefault(x=> x.Id == id);
            return View(Book);
        }
    }
}
