using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Service;

namespace siteNetCore31.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public PagesController(DataManager dataManager, IWebHostEnvironment hostEnvironment)
        {
            this.dataManager = dataManager;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(Guid id)
        {
            var entity = id == default ? new Page() : dataManager.Pages.GetPageById(id);
            return View(entity);
        }
        [HttpPost]
        public IActionResult Edit(Page page, Microsoft.AspNetCore.Http.IFormFile Image)
        {
            if(ModelState.IsValid)
            {
                //получаем все элементы и проверяем есть ли элемент с таким же url
                IQueryable<Page> pages = dataManager.Pages.GetPages();
                foreach (var item in pages)
                {
                    if (item.Id != page.Id && item.Url == page.Url)
                    {
                        ModelState.AddModelError(nameof(Page.Url), "Запись с таким URL уже есть");
                        return View(page);
                    }
                }
                if (Image.IsImage())
                {
                    //записываем в объект путь к картинке
                    page.Image = Image.FileName;

                    //сохраняем картинку
                    using (var img = System.Drawing.Image.FromStream(Image.OpenReadStream(), true, true))
                    {
                        //создаём измененную картинку
                        var i = new Bitmap(img.ScaleAndCrop(1920, 1280, TargetSpot.BottomMiddle));
                        //сохраняем картинку
                        i.SaveAs(Path.Combine(hostEnvironment.WebRootPath, "images/pages/", Image.FileName));
                    }
                }
                //сохраняем запись
                dataManager.Pages.SavePage(page);
                //выходим на главную
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }            
            return View(page);
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            dataManager.Pages.DeletePageById(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

    }
}
