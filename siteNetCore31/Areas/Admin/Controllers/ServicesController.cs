using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LazZiya.ImageResize;
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
            var service = id == default ? new Domain.Entities.Service() { Category = dataManager.Categories.GetCategoryById(new Guid("309035C6-9489-41CA-A395-717243880814")) } : dataManager.Services.GetServiceById(id);            
            var categories = dataManager.Categories.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "H1", service.Category);
            return View(service);
        }
        /// <summary>
        /// Изменение записи сервиса
        /// </summary>
        /// <param name="service">Экземпляр объекта Service</param>
        /// <param name="Image">Файл, отправленный через форму</param>
        /// <returns></returns>
        [HttpPost]        
        public IActionResult Edit(Domain.Entities.Service service, IFormFile Image)
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
                if (Image.IsImage())
                {
                    //записываем в объект путь к картинке
                    service.Image = Image.FileName;

                    //сохраняем картинку
                    using (var img = System.Drawing.Image.FromStream(Image.OpenReadStream(), true, true))
                    {
                        //создаём измененную картинку с шириной 783 пикселя
                        var i = new Bitmap(img.ScaleByWidth(783));
                        //создаём квадратную картинку для превью услуг
                        var square = new Bitmap(img.ScaleAndCrop(180, 167, TargetSpot.MiddleLeft));
                        //создаём квадратную картинку для превью в админке
                        var mini = new Bitmap(img.ScaleAndCrop(78, 35, TargetSpot.Center));
                        //сохраняем картинки
                        i.SaveAs(Path.Combine(hostEnvironment.WebRootPath, "images/services/", Image.FileName));
                        square.SaveAs(Path.Combine(hostEnvironment.WebRootPath, "images/services/", "square-"+Image.FileName));
                        mini.SaveAs(Path.Combine(hostEnvironment.WebRootPath, "images/services/", "mini-"+Image.FileName));
                    }
                }
                service.Category = dataManager.Categories.GetCategoryById(service.Category.Id);
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
