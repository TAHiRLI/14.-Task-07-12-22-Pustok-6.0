using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Pustok.DAL;
using Pustok.Migrations;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Areas.manage.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {

        private readonly PustokDbContext _context;

        public GenreController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            int dataPerPage = 2;
            int totalPage = (int)Math.Ceiling(_context.Genres.Count() / (double)dataPerPage);
            if(page > totalPage)
            {
                return RedirectToAction("Error");
            }

            List<Genre> genreList = _context.Genres.Include(x => x.Books).Skip((page - 1) * dataPerPage).Take(dataPerPage).ToList();



            ViewBag.currentPage = page;
            ViewBag.dataPerPage = dataPerPage;
            ViewBag.totalPage = totalPage;


            return View(genreList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (genre == null)
                return RedirectToAction("Error");

            if ( !ModelState.IsValid)
                return View();

            if( _context.Genres.Any(x => x.Name == genre.Name))
            {
                ModelState.AddModelError("Name", "This name has already been used");
                return View();
            }
        
                 _context.Genres.Add(genre);
                _context.SaveChanges();

          return RedirectToAction("Index");
           
        }
        public IActionResult Edit(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if(genre == null)
                return RedirectToAction("Error");

            return View(genre);
        }
        [HttpPost]
        public IActionResult Edit(Genre genre)
        {
            if(!ModelState.IsValid)
                  return View();
            if(_context.Genres.Any(x=> x.Name == genre.Name && x.Id != genre.Id))
            {
                ModelState.AddModelError("Name", "This genre allready exists");
                return View();
            }
            if(_context.Genres.FirstOrDefault(x => x.Id == genre.Id)==null)
                return RedirectToAction("Error");


            _context.Genres.FirstOrDefault(x => x.Id == genre.Id).Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Genre existingGenre = _context.Genres.Include(x=> x.Books).FirstOrDefault(x=> x.Id == id);
            if (existingGenre == null)
                return RedirectToAction("Error");

            return View(existingGenre);
        }

        [HttpPost]
        public IActionResult Delete(Genre genre)
        {
            if (genre == null)
                return RedirectToAction("Error");


            Genre existingGenre = _context.Genres.Include(x=> x.Books).FirstOrDefault(x => x.Id == genre.Id);
            if (existingGenre == null)
                return RedirectToAction("Error");


            if (!existingGenre.Books.Any(x=> x.GenreId == existingGenre.Id))
            { 
                _context.Genres.Remove(existingGenre);
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
