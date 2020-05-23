using siteNetCore31.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.Abstract
{
    //определяем CRUD поведения для страниц
    public interface iPagesRepository
    {
        IQueryable<Page> GetPages();
        Page GetPageById(Guid id);
        Page GetPageByUrl(string url);
        void SavePage(Page page);
        void DeletePageById(Guid id);
    }
}
