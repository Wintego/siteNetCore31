using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.Abstract
{
    //определяем CRUD поведения для сервисов
    public interface iServicesRepository
    {
        IQueryable<Entities.Service> GetServices();
        Entities.Service GetServiceByUrl(string url);
        Entities.Service GetServiceById(Guid id);
        void SaveService(Entities.Service service);
        void DeleteServiceById(Guid id);
    }
}
