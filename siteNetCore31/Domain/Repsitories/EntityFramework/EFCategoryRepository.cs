using Microsoft.EntityFrameworkCore;
using siteNetCore31.Domain.Entities;
using siteNetCore31.Domain.Repsitories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.EntityFramework
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public EFCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetCategories() => context.Categories.AsNoTracking();

        public void DeleteCategory(Guid id)
        {
            context.Categories.Remove(new Category() { Id = id });
            context.SaveChanges();
        }

        public Category GetCategoryById(Guid id)
        {
            return context.Categories.FirstOrDefault(x => x.Id == id);
        }

        public Category GetCategoryByUrl(string url)
        {
            return context.Categories.FirstOrDefault(x => x.Url == url);
        }

        public void SaveCategory(Category category)
        {
            if (category.Id == default)
            {
                //устанавливаем дату создания
                category.DateCreated = DateTime.UtcNow;
                context.Entry(category).State = EntityState.Added;
            }
            else
            {
                //устанавливаем дату изменения
                category.DateUpdated = DateTime.UtcNow;
                context.Entry(category).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
