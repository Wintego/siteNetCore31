using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        //[HttpGet("{id}", Name = "Info")]
        [Route("{id}", Name = "Info")]
        public IActionResult Info(string id)
        {
            var page = dataManager.Pages.GetPageByUrl(id);
            if (page is Page)
                return View(page);
            else return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        [HttpGet("service/{id}", Name = "Service")]
        public IActionResult Service(string id)
        {
            var page = dataManager.Services.GetServiceByUrl(id);
            return View(page);
        }
        //[HttpGet("{id}", Name = "Category")]
        public IActionResult Category(string id)
        {
            var category = dataManager.Categories.GetCategoryByUrl(id);
            return View(category);
        }
    }
}
