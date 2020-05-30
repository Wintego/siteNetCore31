using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Entities
{
    //базовый класс для всех сущностей
    public abstract class EntityBase
    {

        [Required]
        public Guid Id { get; set; }

        [Display(Name ="URL страницы")]
        public virtual string Url { get; set; }
        
        [Display(Name ="Название (h1)")]
        public virtual string H1 { get; set; }
        
        [Display(Name = "Краткое описание")]
        public virtual string ShortDescription { get; set; }
        
        [Display(Name = "SEO Title")]
        public virtual string seoTitle { get; set; }
        
        [Display(Name = "SEO Description")]
        public virtual string seoDescription { get; set; }
        
        [Display(Name = "Текст")]
        public virtual string Text { get; set; }
        
        [Display(Name = "Главная картинка")]
        public virtual string Image { get; set; }

        [Display(Name = "Название для меню")]
        public virtual string MenuName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateUpdated { get; set; }
    }
}
