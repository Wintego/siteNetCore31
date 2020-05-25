using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var entity = id == default ? new Domain.Entities.Service() : dataManager.Services.GetServiceById(id);
            return View(entity);
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
                if (image != null)
                {
                    //записываем в объект путь к картинке
                    service.Image = image.FileName;
                    //сохраняем картинку
                    using(var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "images/", image.FileName), FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }
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
