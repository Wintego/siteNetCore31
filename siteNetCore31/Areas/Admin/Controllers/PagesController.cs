using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;

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

        public IActionResult Edit(string url)
        {
            var page = dataManager.Pages.GetPageByUrl(url);
            return View(page);
        }
    }
}
