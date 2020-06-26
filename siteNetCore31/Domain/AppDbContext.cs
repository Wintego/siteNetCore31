using Microsoft.AspNetCore.Identity;
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
                Text = @"<div id=""slider"" class=""inspiro-slider slider-fullscreen dots-creative"" data-fade=""true"">
            <div class=""slide"" data-bg-video=""video/ses.mp4"">
                <div class=""bg-overlay""></div>
                <div class=""container"">
                    <div class=""slide-captions text-left text-light"">
                        <h1>Санэпидемстанция №1 по Москве и области</h1>
                        <p class=""text-small"">Официальная СЭС предоставляет населению услуги по уничтожению насекомых, грызунов, оформлению санитарной документации для открытия бизнеса, а так же производит лабораторный анализ воды, воздуха, почвы и продуктов питания.</p>
                        <div><a href=""#welcome"" class=""btn scroll-to"">Подробнее</a></div>
                    </div>
                </div>
            </div>
        </div>
            <section id=""welcome"" class=""p-b-0"">
                <div class=""container"">
                    <div class=""heading-text heading-section text-center m-b-40"" data-animate=""fadeInUp"">
                        <h2>О САНЭПИДЕМСТАНЦИИ</h2>
                        <span class=""lead"">Create amam ipsum dolor sit amet, Beautiful nature, and rare feathers!.</span>
                    </div>
                    <div class=""row"" data-animate=""fadeInUp"">
                        <div class=""col-lg-12"">
                            <img class=""img-fluid"" src=""/images/other/main.png"" alt=""@Config.CompanyName"">
                        </div>
                    </div>
                </div>
            </section>
            <section class=""background-grey"">
                <div class=""container"">
                    <div class=""heading-text heading-section"">
                        <h2>НАШИ ПРЕИМУЩЕСТВА</h2>
                        <span class=""lead"">Create amam ipsum dolor sit amet, Beautiful nature, and rare feathers!.</span>
                    </div>
                    <div class=""row"">
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""0"">
                                <h4>Премиум препараты</h4>
                                <p>Использование исключительно сертифицированных, отличающихся высоким уровнем безопасности для человека и животных химических препаратов.</p>
                            </div>
                        </div>
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""200"">
                                <h4>Индивидуальный подход</h4>
                                <p>Мы всегда проводим предварительную оценку ситуации до того, как начать обработку, предлагаем возможности для выбора препаратов, применяемых при химической дезинсекции или дезинфекции.</p>
                            </div>
                        </div>
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""400"">
                                <h4>Эксперты высшего класса</h4>
                                <p>Мы не нанимаем случайных людей. Каждый сотрудник - настоящий профессионал в своей области, делающий работу добросовестно и качественно.</p>
                            </div>
                        </div>
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""600"">
                                <h4>Гарантия</h4>
                                <p>На все услуги предоставляется гарантия</p>
                            </div>
                        </div>
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""800"">
                                <h4>Цены</h4>
                                <p>Вы можете уточнить наличие действующих скидок и акций у специалиста при заказе услуги по телефону.</p>
                            </div>
                        </div>
                        <div class=""col-lg-4"">
                            <div data-animate=""fadeInUp"" data-animate-delay=""1000"">
                                <h4>Режим работы</h4>
                                <p>Если вы можете принять специалистов только в вечернее время или рано утром - это обязательно будет учтено нашими специалистами.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section>
                <div class=""container"">
                    <div class=""heading-text heading-section text-center"">
                        <h2>Наши услуги</h2>
                        <p>
                            Lorem ipsum dolor sit amet, consecte adipiscing elit. Suspendisse condimentum porttitor cursumus.
                        </p>
                    </div>
                    <div class=""row"">
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""0"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-squirrel""></i>
                                </div>
                                <h3>Дератизация</h3>
                                <p>
                                    Борьба с грызунами в городе и на дачных участках по-прежнему весьма актуальна
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""200"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-bug""></i>
                                </div>
                                <h3>Дезинсекция</h3>
                                <p>
                                    Уничтожение клопов, тараканов и других насекомых в квартире и на участке
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""400"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-viruses""></i>
                                </div>
                                <h3>Дезинфекция</h3>
                                <p>
                                    Патогенные возбудители представляют серьезную опасность и для предприятий общепита, и для обычных людей
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""600"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""far fa-smoking""></i>
                                </div>
                                <h3>Дезодорация</h3>
                                <p>
                                    Уничтожение неприятных запахов в квартире и автомобиле
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""800"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-pallet-alt""></i>
                                </div>
                                <h3>Фумигация</h3>
                                <p>
                                    Подготовка тары к перевозке грузов входит в перечень требований, обязательных для международных перевозок
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""1000"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-flask""></i>
                                </div>
                                <h3>Лабораторные анализы</h3>
                                <p>
                                    Проведение анализов позволяет обеспечить максимальную точность исследования
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""1200"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-file-certificate""></i>
                                </div>
                                <h3>Документация</h3>
                                <p>
                                    Оформление, а также разработка санитарной документации для предприятий и организаций
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""1400"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-toilet""></i>
                                </div>
                                <h3>Канализация</h3>
                                <p>
                                    Чистка труб канализации, устранение засоров
                                </p>
                            </div>
                        </div>
                        <div class=""col-lg-4"" data-animate=""fadeInUp"" data-animate-delay=""1600"">
                            <div class=""icon-box effect medium border small"">
                                <div class=""icon"">
                                    <i class=""fa fa-wind""></i>
                                </div>
                                <h3>Вентиляция</h3>
                                <p>
                                    Очистка вентиляционных систем, воздуховодов, дымоходов
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section class=""text-light p-t-150 p-b-150 "" data-bg-parallax=""images/parallax/12.jpg"">
                <div class=""bg-overlay""></div>
                <div class=""container"">
                    <div class=""row"">
                        <div class=""col-lg-3"">
                            <div class=""text-center"">
                                <div class=""icon""><i class=""fa fa-3x fa-city""></i></div>
                                <div class=""counter""> <span data-speed=""3000"" data-refresh-interval=""50"" data-to=""124162"" data-from=""600"" data-seperator=""true""></span> </div>
                                <div class=""seperator seperator-small""></div>
                                <p>КВАРТИР</p>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""text-center"">
                                <div class=""icon""><i class=""fa fa-3x fa-industry""></i></div>
                                <div class=""counter""> <span data-speed=""4500"" data-refresh-interval=""23"" data-to=""1365"" data-from=""100"" data-seperator=""true""></span> </div>
                                <div class=""seperator seperator-small""></div>
                                <p>ПРЕДПРИЯТИЙ</p>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""text-center"">
                                <div class=""icon""><i class=""fa fa-3x fa-bug""></i></div>
                                <div class=""counter""> <span data-speed=""3000"" data-refresh-interval=""12"" data-to=""23021841"" data-from=""50"" data-seperator=""true""></span> </div>
                                <div class=""seperator seperator-small""></div>
                                <p>НАСЕКОМЫХ УНИЧТОЖЕНО</p>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""text-center"">
                                <div class=""icon""><i class=""fa fa-3x fa-vial""></i></div>
                                <div class=""counter""> <span data-speed=""4550"" data-refresh-interval=""50"" data-to=""14825"" data-from=""48"" data-seperator=""true""></span> </div>
                                <div class=""seperator seperator-small""></div>
                                <p>АНАЛИЗОВ ПРОВЕДЕНО</p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section class=""p-t-60"">
                <div class=""container"">
                    <div class=""heading-text heading-section text-center"">
                        <h2>Наши клиенты</h2>
                        <span class=""lead"">Наши замечательные клиенты, с которыми нам было приятно работать! </span>
                    </div>
                    <div class=""carousel client-logos"" data-items=""6"" data-items-sm=""4"" data-items-xs=""3"" data-items-xxs=""2"" data-margin=""20"" data-arrows=""false"" data-autoplay=""true"" data-autoplay=""3000"" data-loop=""true"">
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/1.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/2.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/3.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/4.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/5.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/6.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/7.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/8.png""> </a>
                        </div>
                        <div>
                            <a href=""#""><img alt="""" src=""images/clients/9.png""> </a>
                        </div>
                    </div>
                </div>
            </section>
            <section class=""background-grey"">
                <div class=""container"">
                    <div class=""heading-text heading-section text-center"">
                        <h2>ПОЗНАКОМЬТЕСЬ С НАШЕЙ КОМАНДОЙ</h2>
                        <p>
                            Lorem ipsum dolor sit amet, consecte adipiscing elit. Suspendisse condimentum porttitor cursumus.
                        </p>
                    </div>
                    <div class=""row team-members"">
                        <div class=""col-lg-3"">
                            <div class=""team-member"">
                                <div class=""team-image"">
                                    <img src=""images/team/1.jpg"">
                                </div>
                                <div class=""team-desc"">
                                    <h3>Дмитрий Сергеевич</h3>
                                    <span>Директор</span>
                                    <p>Организация, координация и контроль работы предприятия </p>
                                    <div class=""align-center"">
                                        <a class=""btn btn-xs btn-slide btn-light"" href=""mailto:director@mail.ru"" data-width=""80"">
                                            <i class=""icon-mail""></i>
                                            <span>Mail</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""team-member"">
                                <div class=""team-image"">
                                    <img src=""images/team/2.jpg"">
                                </div>
                                <div class=""team-desc"">
                                    <h3>Алёна Семёнова</h3>
                                    <span>Менеджер-лаборант</span>
                                    <p>Консультирование клиентов, контроль проведения лабораторных анализов</p>
                                    <div class=""align-center"">
                                        <a class=""btn btn-xs btn-slide btn-light"" target=""_blank"" href=""https://t.me/sesalena"" data-width=""100"">
                                            <i class=""fab fa-telegram""></i>
                                            <span>Telegram</span>
                                        </a>
                                        <a class=""btn btn-xs btn-slide btn-light"" href=""mailto:@Config.CompanyEmail?subject=Для%20Алёны"" data-width=""80"">
                                            <i class=""icon-mail""></i>
                                            <span>Email</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""team-member"">
                                <div class=""team-image"">
                                    <img src=""images/team/3.jpg"">
                                </div>
                                <div class=""team-desc"">
                                    <h3>Екатерина Куравлева</h3>
                                    <span>Менеджер</span>
                                    <p>Приём входящих звонков, консультирование </p>
                                    <div class=""align-center"">
                                        <a class=""btn btn-xs btn-slide btn-light"" target=""_blank"" href=""https://t.me/sesekaterina"" data-width=""100"">
                                            <i class=""fab fa-telegram""></i>
                                            <span>Telegram</span>
                                        </a>
                                        <a class=""btn btn-xs btn-slide btn-light"" href=""mailto:@Config.CompanyEmail?subject=Для%20Екатерины"" data-width=""80"">
                                            <i class=""icon-mail""></i>
                                            <span>Email</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""col-lg-3"">
                            <div class=""team-member"">
                                <div class=""team-image"">
                                    <img src=""images/team/4.jpg"">
                                </div>
                                <div class=""team-desc"">
                                    <h3>Ксения Житова</h3>
                                    <span>Менеджер</span>
                                    <p>Приём входящих звонков, консультирование </p>
                                    <div class=""align-center"">
                                        <a class=""btn btn-xs btn-slide btn-light"" target=""_blank"" href=""https://t.me/seskseniya"" data-width=""100"">
                                            <i class=""fab fa-telegram""></i>
                                            <span>Telegram</span>
                                        </a>
                                        <a class=""btn btn-xs btn-slide btn-light"" href=""mailto:@Config.CompanyEmail?subject=Для%20Ксении"" data-width=""80"">
                                            <i class=""icon-mail""></i>
                                            <span>Email</span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>"
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