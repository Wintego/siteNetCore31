using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Service;

namespace siteNetCore31.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        //доступ к контексту базы данных
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public ServicesController(DataManager dataManager, IWebHostEnvironment hostEnvironment)
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
            //берём объект по Id, или создаем новый, если такого Id нет в базе
            var service = id == default ? new Domain.Entities.Service() : dataManager.Services.GetServiceById(id);

            var categories = dataManager.Categories.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "H1", service.Category);
            return View(service);
        }
        /// <summary>
        /// Изменение записи сервиса
        /// </summary>
        /// <param name="service">Экземпляр объекта Service</param>
        /// <param name="image">Файл, отправленный через форму</param>
        /// <returns></returns>
        [HttpPost]        
        public IActionResult Edit(Domain.Entities.Service service, IFormFile image)
        {
            if(ModelState.IsValid)
            {
                //получаем все элементы и проверяем есть ли элемент с таким же url
                //var ser = dataManager.Services.GetServices().Where(x => x.Id != service.Id).Where(x => x.Url == service.Url).Select();
                IQueryable<Domain.Entities.Service> services = dataManager.Services.GetServices();
                foreach (var item in services)
                {
                    if (item.Id != service.Id && item.Url == service.Url)
                    {
                        ModelState.AddModelError(nameof(Domain.Entities.Service.Url), "Запись с таким URL уже есть");
                        return View(service);
                    }
                }
                //проверяем картинку
                if (image.IsImage())
                {
                    //записываем в объект путь к картинке
                    service.Image = image.FileName;
                    //сохраняем картинку
                    using(var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "images/", image.FileName), FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }
                //не работает?
                //service.Category = dataManager.Categories.GetCategoryById(service.Category.Id);
                dataManager.Services.SaveService(service);
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            dataManager.Services.DeleteServiceById(id);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

    }
}
