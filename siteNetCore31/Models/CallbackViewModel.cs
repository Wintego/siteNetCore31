using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Models
{
    public class CallbackViewModel
    {
        [Required(ErrorMessage = "Укажите номер телефона!", AllowEmptyStrings = false)]
        [Display(Name = "Ваш телефон")]
        public string Phone { get; set; }
        [Display(Name = "Ваше имя")]
        public string Name { get; set; }
    }
}
