using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.BLL.Intarfaces
{
    public interface IGerericRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> SearchByName(string name);
        Task<T> GetAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
