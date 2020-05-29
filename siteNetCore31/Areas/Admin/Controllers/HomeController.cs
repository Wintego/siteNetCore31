using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Service;

namespace siteNetCore31.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DataManager dataManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(DataManager dataManager, IWebHostEnvironment hostEnvironment)
        {
            this.dataManager = dataManager;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View(dataManager);
        }

        //сохранение изображений загруженных через редактор CKEdit4
        [HttpPost]
        public ActionResult CKEdit4Upload(IFormFile upload)
        {
            //проверяем пустой ли файл
            if (upload.Length <= 0) return null;

            //проверяем картинка ли это
            if (!upload.IsImage())
            {
                var NotImageMessage = $"{upload.FileName} не изображение!";
                //формируем ответ
                var NotImage = new { uploaded = 0, error = new { message = NotImageMessage } };
                return Json(NotImage);
            }
            //изменяем название картинки
            var fileName = upload.FileName.ToLower();
            //сохраняем картинку
            using (var stream = new FileStream(Path.Combine(hostEnvironment.WebRootPath, "images/content/", fileName), FileMode.Create))
            {
                upload.CopyTo(stream);
            }
            //путь к картинке для ответа
            var url = $"/images/content/{fileName}";
            var successMessage = "Изображение успешно загружено";
            //формируем ответ
            var success = new { uploaded = 1, fileName, url, error = new { message = successMessage } };
            return Json(success);
        }
    }
}
