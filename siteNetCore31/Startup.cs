using System;
using System.Collections.Generic;
using System.Linq;
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
            //���������� ������ �� appsettings.json
            Configuration.Bind("Project", new Config());

            //���������� ���������� ���������� � �������� ��������
            services.AddTransient<iPagesRepository, EFPagesRepository>();
            services.AddTransient<iServicesRepository, EFServicesRepository>();
            services.AddTransient<DataManager>();
            
            //���������� �������� ���� ������
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionStringLocal));

            //����������� ������� identity
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

            //������������ ������ ��������� ������������ � ������������� (MVC)
            services.AddControllersWithViews()
                //������������� ������������� � apn.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //���������� �������������
            app.UseRouting();

            //���������� ����������� �����
            app.UseStaticFiles();

            //���������� ������������ � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //������ �������� (���������)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
