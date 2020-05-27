using siteNetCore31.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Entities.Category> GetCategories();
        Category GetCategoryById(Guid id);
        Category GetCategoryByUrl(string url);
        void SaveCategory(Category category);
        void DeleteCategory(Guid id);
    }
}
