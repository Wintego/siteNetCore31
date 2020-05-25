﻿using siteNetCore31.Domain.Repsitories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories
{
    //обслуживающий класс, в котором содержится управление репозиторием
    public class DataManager
    {
        public DataManager(iPagesRepository pages, iServicesRepository services)
        {
            Pages = pages;
            Services = services;
        }

        public iPagesRepository Pages { get; set; }
        public iServicesRepository Services { get; set; }
    }
}