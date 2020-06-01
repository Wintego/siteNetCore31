using Microsoft.EntityFrameworkCore;
using siteNetCore31.Domain.Repsitories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace siteNetCore31.Domain.Repsitories.EntityFramework
{
    //реализуем интерфейс управления сервисами
    public class EFServicesRepository : iServicesRepository
    {
        //доступ к базе данных
        private readonly AppDbContext context;

        public EFServicesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void DeleteServiceById(Guid id)
        {
            //удаляем передавая фейковый экземляр объекта с тем же Id
            //по которому будет произведено удаление записи с этим Id
            context.Services.Remove(new Entities.Service()
            {
                Id = id
            });
            //сохраняем изменения в базе
            context.SaveChanges();
        }

        public Entities.Service GetServiceById(Guid id)
        {
            return context.Services.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public Entities.Service GetServiceByUrl(string url)
        {
            return context.Services.Include(x => x.Category).FirstOrDefault(x => x.Url == url);
        }

        public IQueryable<Entities.Service> GetServices()
        {
            return context.Services.Include(x=>x.Category).AsNoTracking();
        }

        //сохранение и изменение объекта
        public void SaveService(Entities.Service service)
        {
            //если service.Id == default значит это новая запись
            if (service.Id == default)
            {
                //устанавливаем дату создания
                service.DateCreated = DateTime.UtcNow;
                //устанавливаем дату изменения
                service.DateUpdated = service.DateCreated;
                //помечаем ключем, что это новый объект
                context.Entry(service).State = EntityState.Added;
            }            
            else
            {
                //устанавливаем дату изменения
                service.DateUpdated = DateTime.UtcNow;
                //помечаем ключом, что это измененный объект
                context.Entry(service).State = EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
