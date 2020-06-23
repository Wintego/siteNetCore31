using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using siteNetCore31.Domain;
using siteNetCore31.Domain.Repsitories;
using siteNetCore31.Domain.Repsitories.Abstract;
using siteNetCore31.Domain.Repsitories.EntityFramework;
using siteNetCore31.Service;

namespace siteNetCore31
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //изменяем кодировку
            services.AddWebEncoders(o => { o.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(UnicodeRanges.All); });

            //подключаем когфиг из appsettings.json
            Configuration.Bind("Project", new Config());
            Configuration.Bind("Email", new Email());

            //подключаем функционал приложения в качестве сервисов
            services.AddTransient<iPagesRepository, EFPagesRepository>();
            services.AddTransient<iServicesRepository, EFServicesRepository>();
            services.AddTransient<ICategoryRepository, EFCategoryRepository>();
            services.AddTransient<DataManager>();
            
            //подключаем контекст базы данных
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionStringLocal));

            //настраиваем систему identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "authCookie";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });
            //добавляем политики авторизации
            services.AddAuthorization(x =>
            {
                //добавляем политику с названием AdminArea, которая требует роль admin
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });

            //регистрируем сервис поддержки контроллеров и представлений (MVC)
            services.AddControllersWithViews(x=>
            {
                x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            })
                //устанавливаем совместимость с apn.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }
            //вывод ошибок
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            //используем статические файлы
            app.UseStaticFiles();

            //используем маршрутизацию
            app.UseRouting();

            //подключаем аунтефикацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //задаем маршруты (ендпоинты)
            app.UseEndpoints(endpoints =>
            {
                //добавляем путь к зоне администратора
                endpoints.MapControllerRoute("admin","{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //путь по умолчанию
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
