using MVCProject.BLL.Intarfaces;
using MVCProject_DAL.Data;
using MVCProject_DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        //public IEmployeeRepository EmployeeRepository { get; set; }
        //public IDepartmentRepository DepartmentRepository { get; set; }
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
            //EmployeeRepository = new EmployeeRepository(dbContext);
            //DepartmentRepository = new DepartmentRepository(dbContext);
        }


        public IGerericRepository<T> Repository<T>() where T : ModelBase
        {
            var Key =typeof(T).Name;
            if(!_repositories.ContainsKey(Key))
            {
                if(Key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(Key, repository);
                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    _repositories.Add(Key, repository);
                }
            }
            return _repositories[Key] as IGerericRepository<T>;
        }
        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _dbContext.DisposeAsync();
        }

       
    }
}
