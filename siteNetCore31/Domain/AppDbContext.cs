﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
                Id = new Guid("E50E02EC-486C-4CE0-8036-3489A166766"),
                Url = "index",
                H1 = "Главная"
            });
        }
    }
}