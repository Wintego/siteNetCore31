using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public CategoriesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
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
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                dataManager.Categories.SaveCategory(category);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

        }
    }
}
