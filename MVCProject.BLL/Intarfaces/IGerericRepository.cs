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
        IEnumerable<T> GetAll();
        IQueryable<T> SearchByName(string name);
        T Get(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
