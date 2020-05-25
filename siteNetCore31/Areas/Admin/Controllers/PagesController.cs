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
                dataManager.Pages.SavePage(page);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }            
            return View(page);
        }

    }
}
