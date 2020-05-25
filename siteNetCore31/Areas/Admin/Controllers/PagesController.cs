using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public PagesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
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
        public IActionResult Edit(Page page)
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
