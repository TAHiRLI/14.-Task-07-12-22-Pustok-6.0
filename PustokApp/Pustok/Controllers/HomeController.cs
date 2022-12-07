using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers
{
    public class HomeController : Controller
    {
        private readonly PustokDbContext _context;

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel HomeVM = new HomeViewModel
            {
                SpecialBooks = _context.Books.Include(x => x.Author).Include(x=> x.BookImages).Where(x => x.IsSpecial).Take(20).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsNew).Take(20).ToList(),
                DiscountedBooks = _context.Books.Include(x=> x.Author).Include(x => x.BookImages).Where(x=> x.DiscountPercent > 0).Take(20).ToList(),
                HeroSlider = _context.Sliders.OrderBy(x=>x.Order).ToList(),
                HomeFeatures = _context.HomeFeatures.Take(4).ToList(),
                Settings = _context.Settings.ToDictionary(x=> x.Key, x=> x.Value)
            };
            return View(HomeVM);
        }

       
    }
}
