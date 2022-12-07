using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Areas.manage.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly PustokDbContext _context;

        public AuthorController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            int dataPerPage = 2;
            int totalPage = (int)Math.Ceiling(_context.Authors.Count() / (double)dataPerPage);
            if (page > totalPage)
            {
                return RedirectToAction("Error");
            }
            List<Author> authorList = _context.Authors.Skip((page-1)*dataPerPage).Take(dataPerPage).ToList();


            ViewBag.currentPage = page;
            ViewBag.dataPerPage = dataPerPage;
            ViewBag.totalPage = totalPage;

            return View(authorList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (author == null)
                return RedirectToAction("Error");

            if (!ModelState.IsValid)
                return View();

            if (_context.Authors.Any(x => x.FullName == author.FullName))
            {
                ModelState.AddModelError("FullName", "This name has already been used");
                return View();
            }

            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult Edit(int id)
        {
            Author Author = _context.Authors.FirstOrDefault(x => x.Id == id);
            if (Author == null)
                return RedirectToAction("Error");

            return View(Author);
        }
        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (!ModelState.IsValid)
                return View();
            if (_context.Authors.Any(x => x.FullName == author.FullName && x.Id != author.Id))
            {
                ModelState.AddModelError("FullName", "This Author allready exists");
                return View();
            }
            if (_context.Authors.FirstOrDefault(x => x.Id == author.Id) == null)
                return RedirectToAction("Error");


            _context.Authors.FirstOrDefault(x => x.Id == author.Id).FullName = author.FullName;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            Author existingAuthor = _context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == id);
            if (existingAuthor == null)
                return RedirectToAction("Error");

            return View(existingAuthor);
        }

        [HttpPost]
        public IActionResult Delete(Author Author)
        {
            if (Author == null)
                return RedirectToAction("Error");


            Author existingAuthor = _context.Authors.Include(x => x.Books).FirstOrDefault(x => x.Id == Author.Id);
            if (existingAuthor == null)
                return RedirectToAction("Error");


            if (!existingAuthor.Books.Any(x => x.AuthorId == existingAuthor.Id))
            {
                _context.Authors.Remove(existingAuthor);
                _context.SaveChanges();

            }


            return RedirectToAction("Index");


        }




        public IActionResult Error()
        {
            return View();
        }

    }
}
