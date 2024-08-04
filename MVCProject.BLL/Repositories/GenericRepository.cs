using Microsoft.EntityFrameworkCore;
using MVCProject.BLL.Intarfaces;
using MVCProject_DAL.Data;
using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.BLL.Repositories
{
    public class GenericRepository<T>: IGerericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;
        public  GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
         =>  _dbContext.Set<T>().Add(entity);        //_dbContext.Add(entity);
        public void Update(T entity)
         => _dbContext.Set<T>().Update(entity);     //_dbContext.Update(entity);
           

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);  //_dbContext.Remove(entity);


        public async Task<T> GetAsync(int id)
        {
            ///var department =_dbContext.Departments.Local.Where(D =>D.Id==id).FirstOrDefault();
            ///if (department == null)
            ///    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            ///return department;
            if (typeof(T) == typeof(Employee))
                return _dbContext.Employees.Include(E => E.Department).Where(E => E.Id == id).FirstOrDefault() as T;
            else
                return await _dbContext.FindAsync<T>(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await _dbContext.Employees.Include(E =>E.Department).AsNoTracking().ToListAsync();
            else
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync(); 
        }

        public IQueryable<T> SearchByName(string name)
         => _dbContext.Set<T>().Where(E => E.Name.ToLower().Contains(name));
    }
}
