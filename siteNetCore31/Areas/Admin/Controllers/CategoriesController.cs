using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Service;

namespace siteNetCore31.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public CategoriesController(DataManager dataManager, IWebHostEnvironment hostEnvironment)
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
            var entity = id == default ? new Category() : dataManager.Categories.GetCategoryById(id);
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var thisCategory = dataManager.Categories.GetCategoryById(id);
            //получаем категорию по умолчанию
            var defaultCategory = dataManager.Categories.GetCategoryById(new Guid("309035C6-9489-41CA-A395-717243880814"));
            //получаем все сервисы в IList
            IList<Domain.Entities.Service> services = await dataManager.Services.GetServices().Where(x => x.Category == thisCategory).ToListAsync();
            //устанавливаем категорию в данных сервисах по умолчанию и сохраняем в базу
            foreach (var service in services)
            {
                service.Category = defaultCategory;
                dataManager.Services.SaveService(service);
            }
            //сохраняем категорию чтобы не отслеживать изменения
            dataManager.Categories.SaveCategory(thisCategory);
            //удаляем категорию
            dataManager.Categories.DeleteCategory(thisCategory);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        [HttpPost]
        public IActionResult Edit(Category category, IFormFile Image)
        {
            if (ModelState.IsValid)
            {                
                if (Image.IsImage())
                {
                    //записываем в объект путь к картинке
                    category.Image = Image.FileName;

                    //сохраняем картинку
                    using (var img = System.Drawing.Image.FromStream(Image.OpenReadStream(), true, true))
                    {
                        //создаём измененную картинку
                        var i = new Bitmap(img.ScaleAndCrop(1920, 1280, TargetSpot.BottomMiddle));
                        //сохраняем картинки
                        i.SaveAs(Path.Combine(hostEnvironment.WebRootPath, "images/categories/", Image.FileName));
                    }
                }
                dataManager.Categories.SaveCategory(category);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

        }
    }
}
