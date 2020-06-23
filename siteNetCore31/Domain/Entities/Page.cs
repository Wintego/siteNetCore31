using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Entities
{
    public class Page : EntityBase
    {
        [Required]
        [Display(Name = "Название страницы (h1)")]
        public override string H1 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Введите адрес страницы!")]
        [Display(Name = "URL страницы")]
        public override string Url { get; set; }
    }
}
