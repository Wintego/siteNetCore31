using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain
{
    //создание контекста базы данных
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        //создание таблиц доменных объектов
        public DbSet<Entities.Page> Pages { get; set; }
        public DbSet<Entities.Service> Services { get; set; }
        public DbSet<Entities.Category> Categories { get; set; }

        //при создании базы заполняем её значениями по умолчанию
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //создаём таблицу ролей с ролью admin
            builder.Entity<IdentityRole>()
                //если такой сущности нет в базе, то создаём новую
                .HasData(new IdentityRole
            {
                Id = "DCCC3E92-3165-4807-A95D-F8BB0E4270A3",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            //создаём таблицу пользователей с записью пользователя netcore
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "8704E50D-8A81-4CFD-BE85-8C1540FC1BF6",
                UserName = "netcore",
                NormalizedUserName = "NETCORE",
                Email = Config.CompanyEmail,
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "netcorePassword"),
                SecurityStamp = String.Empty
            });

            //создаём таблицу с записью связывания пользователя netcore с ролью admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "DCCC3E92-3165-4807-A95D-F8BB0E4270A3",
                UserId = "8704E50D-8A81-4CFD-BE85-8C1540FC1BF6"
            });

            //добавляем страницу Услуги
            builder.Entity<Entities.Page>().HasData(new Entities.Page
            {
                Id = new Guid("27C6FDEB-2783-4F3E-83FE-064E0582B175"),
                Url = "services",
                H1 = "Услуги"
            });

            //добавляем страницу Главная
            builder.Entity<Entities.Page>().HasData(new Entities.Page
            {
                Id = new Guid("A241DD18-E497-40BE-8504-4DEAACA2C6CF"),
                Url = "index",
                H1 = "Главная"
            });
            builder.Entity<Category>().HasData(new Entities.Category
            {
                Id = new Guid("309035C6-9489-41CA-A395-717243880814"),
                Url = "default",
                H1 = "По умолчанию"
            });
            builder.Entity<Entities.Service>().HasData(new Entities.Service
            {
                Id = new Guid("666599D8-EAC4-4F43-9F15-B7063C583B76"),
                Url = "usluga-1",
                H1 = "Услуга 1"
            });
        }
    }
}