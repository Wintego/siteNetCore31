﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            DateTime date = DateTime.UtcNow;
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

            //добавляем главную страницу
            builder.Entity<Entities.Page>().HasData(new Entities.Page
            {
                Id = new Guid("A241DD18-E497-40BE-8504-4DEAACA2C6CF"),
                Url = "index",
                H1 = "Главная",
                Text = @"<div class=""wrapper style2"">
						<div class=""inner"">
								<section class=""container box feature2"">
									<div class=""row"">
										<div class=""col-6 col-12-medium"">
											<section>
												<header class=""major"">
													<h2>And this is a subheading</h2>
													<p>It’s important but clearly not *that* important</p>
												</header>
												<p>Phasellus quam turpis, feugiat sit amet ornare in, hendrerit in lectus.
												Praesent semper mod quis eget mi. Etiam eu ante risus. Aliquam erat volutpat.
												Aliquam luctus et mattis lectus sit amet pulvinar. Nam turpis nisi
												consequat etiam.</p>
												<footer>
													<a href=""#"" class=""button medium icon solid fa-arrow-circle-right"">Let's do this</a>
												</footer>
											</section>
										</div>
										<div class=""col-6 col-12-medium"">
											<section>
												<header class=""major"">
													<h2>This is also a subheading</h2>
													<p>And is as unimportant as the other one</p>
												</header>
												<p>Phasellus quam turpis, feugiat sit amet ornare in, hendrerit in lectus.
												Praesent semper mod quis eget mi. Etiam eu ante risus. Aliquam erat volutpat.
												Aliquam luctus et mattis lectus sit amet pulvinar. Nam turpis nisi
												consequat etiam.</p>
												<footer>
													<a href=""#"" class=""button medium alt icon solid fa-info-circle"">Wait, what?</a>
												</footer>
											</section>
										</div>
									</div>
								</section>
							</div>
					</div>"
            });
            builder.Entity<Category>().HasData(new Entities.Category
            {
                Id = new Guid("309035C6-9489-41CA-A395-717243880814"),
                Url = "default",
                H1 = "По умолчанию",
                DateCreated = date,
                DateUpdated = date
            });
            builder.Entity<Entities.Service>().HasData(new
            {
                Id = new Guid("666599D8-EAC4-4F43-9F15-B7063C583B76"),
                Url = "usluga-1",
                H1 = "Услуга 1",
                CategoryId = new Guid("309035C6-9489-41CA-A395-717243880814"),
                DateCreated = date,
                DateUpdated = date
            });
        }
    }
}