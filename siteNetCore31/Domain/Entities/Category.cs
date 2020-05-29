﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Entities
{
    public class Category : EntityBase
    {
        [Display(Name = "Услуги")]
        public virtual IEnumerable<Service> Services { get; set; }
    }
}