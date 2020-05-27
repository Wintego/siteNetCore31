using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Delete(Guid id)
        {
            dataManager.Categories.DeleteCategory(id);
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
