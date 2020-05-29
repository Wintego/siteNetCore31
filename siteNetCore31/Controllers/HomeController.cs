using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Service;

namespace siteNetCore31.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public IActionResult Index()
        {
            var page = dataManager.Pages.GetPageByUrl("index");
            return View(page);
        }
        [Route("{id}-info", Name = "Info")]
        public IActionResult Info(string id)
        {
            var page = dataManager.Pages.GetPageByUrl(id);
            return View(page);
        }

        [Route("{category}/{id}", Name = "Service")]
        public IActionResult Service(string id, string category)
        {
            var service = dataManager.Services.GetServiceByUrl(id);
            if(service is Domain.Entities.Service)
                return View(service);
            else return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        [Route("{id}", Name = "Category")]
        public IActionResult Category(string id)
        {
            var category = dataManager.Categories.GetCategoryByUrl(id);
            ViewBag.Services = dataManager.Services.GetServices().Where(x => x.Category == category);
            return View(category);
        }
        public IActionResult ReturnImage(string file)
        {
            return File($"~/images/{file}", "image");
        }
    }
}
