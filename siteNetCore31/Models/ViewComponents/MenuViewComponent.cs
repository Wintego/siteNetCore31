using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using siteNetCore31.Domain.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Models.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DataManager dataManager;

        public MenuViewComponent(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default", dataManager));
        }
    }
}
