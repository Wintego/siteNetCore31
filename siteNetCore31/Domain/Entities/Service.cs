using siteNetCore31.Domain.Repsitories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Entities
{
    public class Service : EntityBase
    {
        [Required]
        [Display(Name = "Название услуги (h1)")]
        public override string H1 { get; set; }
        [Display(Name = "Категория")]
        public virtual Category Category { get; set; }
    }
}
