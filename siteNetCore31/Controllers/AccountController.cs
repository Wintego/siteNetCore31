using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using siteNetCore31.Models;

namespace siteNetCore31.Controllers
{
    //для данного контроллера действует правило авторизации
    [Authorize]
    public class AccountController : Controller
    {
        //контекст базы для работы с пользователями
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }        
        public IActionResult Index()
        {
            return View();
        }

        //чтобы залогиниться, нужно быть анонимным
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        //обработка данных из формы авторизации
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //если модель верна
            if(ModelState.IsValid)
            {
                //находим пользователя по указанному в модели логину
                IdentityUser user = await userManager.FindByNameAsync(model.login);
                //если пользователь найден
                if(user != null)
                {
                    //принудительно разлогиниваемся
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(user, model.password, model.remember, false);
                    //если авторизация прошла, редиректим на предыдущую страницу, или на главную
                    if (result.Succeeded) return Redirect(returnUrl ?? "/");
                }
                //если авторизация не прошла, отправляем ошибку на форму
                ModelState.AddModelError(nameof(LoginViewModel), "Неверный логин или пароль");
            }
            //возвращаем инвалидную модель
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
