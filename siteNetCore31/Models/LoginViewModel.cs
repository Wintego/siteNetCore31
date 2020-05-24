using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Заполните логин!", AllowEmptyStrings =false)]
        [Display(Name = "Логин")]
        public string login { get; set; }

        [Required(ErrorMessage = "Заполните пароль!", AllowEmptyStrings = false)]
        [Display(Name = "Пароль")]
        [UIHint("password")]
        public string password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool remember { get; set; }
    }
}
