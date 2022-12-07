using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Pustok.DAL;
using Pustok.Helpers;
using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        
        public IActionResult Index()
        {
            List<Slider> model = _context.Sliders.OrderBy(x=> x.Order).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            // recommend the slider order

            Slider lastSlide = _context.Sliders.OrderByDescending(x => x.Order).FirstOrDefault();
            ViewBag.LastSlide = lastSlide.Order;


            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.File == null)
                ModelState.AddModelError("File", "This field is required");
            if (slider.File != null  && slider.File.ContentType != "image/jpeg" && slider.File.ContentType != "image/png")
                ModelState.AddModelError("File", "Only Jpeg, Jpg and Png format is supported");
            if (slider.File != null &&  slider.File.Length > 2097152)
                ModelState.AddModelError("File", "File size must be smaller than 2MB");

            if (!ModelState.IsValid)
            {
                Slider lastSlide = _context.Sliders.OrderByDescending(x => x.Order).FirstOrDefault();
                ViewBag.LastSlide = lastSlide.Order;

                return View();
            }
           

            if (slider.Order == null || slider.Order < 0)
                slider.Order = 1;

            List<Slider> sliderList = _context.Sliders.Where(x => x.Order >= slider.Order && x.Id != slider.Id).ToList();
            foreach (var item in sliderList)
            {
                item.Order++;
            }

            slider.Image = FileManager.Save(slider.File, _env.WebRootPath, "Uploads/Sliders", 200);
         
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {

            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
                return NotFound();

            FileManager.Delete(_env.WebRootPath, "Uploads/Sliders", slider.Image);

            _context.Sliders.Remove(slider);
            _context.SaveChanges();


            return Ok();
        }
        public IActionResult Edit(int id)
        {
            var Model = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (Model == null)
                return RedirectToAction("Error");


            return View(Model);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider existingSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existingSlider == null)
                return RedirectToAction("Error");


            if (slider.File != null && slider.File.ContentType != "image/jpeg" && slider.File.ContentType != "image/png")
                ModelState.AddModelError("File", "File Type must be Jpeg, Jpg or Png");
            if (slider.File != null && slider.File.Length > 2097152)
                ModelState.AddModelError("File", "File size must be smaller than 2MB");


            if (!ModelState.IsValid)
            {
                return View();
            }


            if(slider.File != null)
            {

                if(existingSlider.Image != null)
                FileManager.Delete(_env.WebRootPath, "Uploads/Sliders", existingSlider.Image);
                string newFileName = FileManager.Save(slider.File, _env.WebRootPath, "Uploads/Sliders", 200);

                existingSlider.Image = newFileName;
            }


            existingSlider.Title1 = slider.Title1;
            existingSlider.Title2 = slider.Title2;
            existingSlider.Desc = slider.Desc;
            existingSlider.BtnText = slider.BtnText;
            if (slider.Order == null || slider.Order < 0)
                slider.Order = 1;
            existingSlider.Order = slider.Order;
            existingSlider.RedirectedUrl = slider.RedirectedUrl;

            List<Slider> sliderList = _context.Sliders.Where(x => x.Order >= slider.Order && x.Id != slider.Id).ToList();
            foreach (var item in sliderList)
            {
                item.Order++;
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
