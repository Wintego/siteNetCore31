using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Service
{
    public class AdminAreaAuthorization : IControllerModelConvention
    {
        private readonly string area;
        private readonly string policy;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="area">Область, для которой будет действовать политика</param>
        /// <param name="policy">Политика для этой области</param>
        public AdminAreaAuthorization(string area, string policy)
        {
            this.area = area;
            this.policy = policy;
        }
        //проверяем аттрибуты у контроллера
        public void Apply(ControllerModel controller)
        {
            //если присутствует аттрибут area
            if(controller.Attributes.Any( a => 
            a is AreaAttribute 
            && (a as AreaAttribute).RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase))
            || controller.RouteValues.Any(r=>  r.Key.Equals("area", StringComparison.OrdinalIgnoreCase)
            && r.Value.Equals(area, StringComparison.OrdinalIgnoreCase)))
            {
                //то топравляем пользователя на авторизацию
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }
    }
}
