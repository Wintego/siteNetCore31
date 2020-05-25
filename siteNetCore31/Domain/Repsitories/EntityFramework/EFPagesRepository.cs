using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.EntityFramework
{
    //реализуем интерфейс управления страницами
    public class EFPagesRepository : iPagesRepository
    {
        //доступ к базе данных
        private readonly AppDbContext context;

        public EFPagesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void DeletePageById(Guid id)
        {
            //удаляем передавая фейковый экземляр объекта с тем же Id
            //по которому будет произведено удаление записи с этим Id
            context.Pages.Remove(new Page() { Id = id });
            //сохраняем изменения в базе
            context.SaveChanges();
        }

        public Page GetPageById(Guid id)
        {
            return context.Pages.FirstOrDefault(x => x.Id == id);
        }

        public Page GetPageByUrl(string url)
        {
            return context.Pages.FirstOrDefault(x => x.Url == url);
        }

        public IQueryable<Page> GetPages()
        {
            return context.Pages.AsNoTracking();
        }

        //сохранение и изменение объекта
        public void SavePage(Page page)
        {
            //context.Pages.Count(x => x.Url == page.Url);
            //если page.Id == default значит это новая запись
            if (page.Id == default)
            {
                //помечаем ключем, что это новый объект
                context.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            //помечаем ключом, что это измененный объект
            else context.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
