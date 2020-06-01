using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Models;
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
            if (id == "callback")
            {
                ViewBag.returnUrl = Request.Headers["Referer"].ToString();
                return View("Callback");
            }
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

        [HttpPost]
        public IActionResult Callback(CallbackViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //пытаемся отправить данные из формы
                try
                {
                    //создаём объект сообщение
                    MimeMessage message = new MimeMessage();
                    //отправитель
                    message.From.Add(new MailboxAddress(Config.CompanyName, Email.From));
                    //куда отправляем
                    message.To.Add(MailboxAddress.Parse(Email.To));
                    //тема письма
                    message.Subject = $"Заказ звонка - {Config.CompanyName}";
                    //текст письма
                    message.Body = new BodyBuilder()
                    {
                        TextBody = $"{model.Phone} {model.Name}"
                    }.ToMessageBody();
                    //отправляем
                    using (SmtpClient client = new SmtpClient())
                    {
                        //данные сервера
                        client.Connect(Email.SMTPServer, Email.SMTPPort, Email.SMTPSSL);
                        //данные авторизации
                        client.Authenticate(Email.From, Email.FromPassword);
                        //отправка
                        client.Send(message);
                    }
                    ViewBag.Message = "Ваш запрос отправлен. Ожидайте звонка!";
                }
                catch
                {
                    ViewBag.Message = "Запрос НЕ отправлен. Перезвоните по номеру "+ Config.CompanyName;
                }                
                ViewBag.returnUrl = returnUrl;
            }            
            return View(model);
        }
    }
}
