using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Pustok.Areas.Admin.ViewModels;
using Pustok.DAL;
using Pustok.Helpers;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {

            var query = _context.Books
                .Include(x => x.Author)
                .Include(x => x.BookImages)
                .Include(x => x.Genre);

            var model = PaginatedList<Book>.Create(query, page, 10);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {

            // CheckImg(book.ImageFiles, book.PosterImg, book.HoverImg);

            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.Authors = _context.Authors.ToList();
                return View();
            }


            // saving the images 
            book.BookImages = new List<BookImage>();



            BookImage bookPoster = new BookImage
            {
                Book = book,
                PosterStatus = true,
                Image = FileManager.Save(book.PosterImg, _env.WebRootPath, "Uploads/Books", 100)
            };
            book.BookImages.Add(bookPoster);
            BookImage bookHover = new BookImage
            {
                Book = book,
                PosterStatus = false,
                Image = FileManager.Save(book.HoverImg, _env.WebRootPath, "Uploads/Books", 100)
            };
            book.BookImages.Add(bookHover);


            if (book.ImageFiles != null)
            {
                foreach (var img in book.ImageFiles)
                {
                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        PosterStatus = null,
                        Image = FileManager.Save(img, _env.WebRootPath, "Uploads/Books", 100)
                    };
                    book.BookImages.Add(bookImage);
                }
            }

            book.CreatedAt = DateTime.UtcNow.AddHours(4);
            book.ModifiedAt = DateTime.UtcNow.AddHours(4);

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {


            var model = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Include(x => x.Genre).FirstOrDefault(x => x.Id == id);
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            // CheckImgEdit(book.ImageFiles, book.PosterImg, book.HoverImg);
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.Authors = _context.Authors.ToList();
                var model = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Include(x => x.Genre).FirstOrDefault(x => x.Id == book.Id);

                return View(model);
            }




            if (book.PosterImg != null)
            {
                // remove old one
                BookImage oldBookImage = _context.BookImages.FirstOrDefault(x => x.PosterStatus == true && x.Book.Id == book.Id);
                if (oldBookImage != null)
                {
                    FileManager.Delete(_env.WebRootPath, "Uploads/Books", oldBookImage.Image);
                    _context.BookImages.Remove(oldBookImage);
                }

                //save new one
                BookImage bookPoster = new BookImage
                {
                    Book = book,
                    PosterStatus = true,
                    Image = FileManager.Save(book.PosterImg, _env.WebRootPath, "Uploads/Books", 100)
                };
                _context.BookImages.Add(bookPoster);


            }

            if (book.HoverImg != null)
            {
                // remove old one
                BookImage oldBookImage = _context.BookImages.FirstOrDefault(x => x.PosterStatus == false && x.Book.Id == book.Id);
                if (oldBookImage != null)
                {
                    FileManager.Delete(_env.WebRootPath, "Uploads/Books", oldBookImage.Image);
                    _context.BookImages.Remove(oldBookImage);

                }

                // save new one
                BookImage bookHover = new BookImage
                {
                    Book = book,
                    PosterStatus = false,
                    Image = FileManager.Save(book.HoverImg, _env.WebRootPath, "Uploads/Books", 100)
                };
                _context.BookImages.Add(bookHover);

            }

            if (book.ImageFiles != null)
            {
                foreach (var img in book.ImageFiles)
                {
                    BookImage bookImage = new BookImage
                    {
                        BookId = book.Id,
                        PosterStatus = null,
                        Image = FileManager.Save(img, _env.WebRootPath, "Uploads/Books", 100)
                    };
                    _context.BookImages.Add(bookImage);
                }
            }




            if (book.BookImageIds == null)
            {
                book.BookImageIds = new List<int>();
            }
            List<int> oldBookImages = _context.BookImages.Where(x => x.BookId == book.Id && x.PosterStatus ==  null).Select(x => x.Id).ToList();

            List<int> removedBookImgs = oldBookImages.AsQueryable().Except(book.BookImageIds).ToList();

          

             // delete the removed images
            foreach (var bkImgId in removedBookImgs)
            {
                var imageName = _context.BookImages.FirstOrDefault(x => x.Id == bkImgId ).Image;
                if (imageName != null)
                {
                    FileManager.Delete(_env.WebRootPath, "Uploads/Books", imageName);
                    _context.BookImages.Remove(_context.BookImages.FirstOrDefault(x => x.Id == bkImgId));

                }
            }






            book.ModifiedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        private void CheckImg(List<IFormFile>? imageFiles, IFormFile posterImg, IFormFile hoverImg)
        {
            if (posterImg == null)
                ModelState.AddModelError("PosterImg", "This field is Required");
            else if (posterImg.ContentType != "image/jpeg" && posterImg.ContentType != "image/png")
                ModelState.AddModelError("PosterImg", "File Type Must Be JPEG, JPG or PNG");
            else if (posterImg.Length > 2097152)
                ModelState.AddModelError("PosterImg", "File size must be less than 2MB!");


            if (hoverImg == null)
                ModelState.AddModelError("HoverImg", "This field is Required");
            else if (hoverImg.ContentType != "image/jpeg" && hoverImg.ContentType != "image/png")
                ModelState.AddModelError("PosterImg", "File test tes tes  Type Must Be JPEG, JPG or PNG");
            else if (hoverImg.Length > 2097152)
                ModelState.AddModelError("HoverImg", "File size must be less than 2MB!");

            if (imageFiles != null)
                foreach (var file in imageFiles)
                {
                    if (file.ContentType != "image/jpeg" && hoverImg.ContentType != "image/png")
                        ModelState.AddModelError("HoverImg", "File Type Must Be JPEG, JPG or PNG");
                    else if (file.Length > 2097152)
                        ModelState.AddModelError("HoverImg", "File size must be less than 2MB!");
                }
        }
        private void CheckImgEdit(List<IFormFile> imageFiles, IFormFile posterImg, IFormFile hoverImg)
        {

            if (posterImg != null && posterImg.ContentType != "image/jpeg" && posterImg.ContentType != "image/png")
                ModelState.AddModelError("PosterImg", "File Type Must Be JPEG, JPG or PNG");
            if (posterImg != null && posterImg.Length > 2097152)
                ModelState.AddModelError("PosterImg", "File size must be less than 2MB!");



            if (hoverImg != null && hoverImg.ContentType != "image/jpeg" && hoverImg.ContentType != "image/png")
                ModelState.AddModelError("HoverImg", "File Type Must Be JPEG, JPG or PNG");
            if (hoverImg != null && hoverImg.Length > 2097152)
                ModelState.AddModelError("HoverImg", "File size must be less than 2MB!");

            if (imageFiles != null)
                foreach (var file in imageFiles)
                {
                    if (file.ContentType != "image/jpeg" && hoverImg.ContentType != "image/png")
                        ModelState.AddModelError("HoverImg", "File Type Must Be JPEG, JPG or PNG");
                    else if (file.Length > 2097152)
                        ModelState.AddModelError("HoverImg", "File size must be less than 2MB!");
                }
        }

    }
}

