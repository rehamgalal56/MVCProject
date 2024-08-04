using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.BLL.Intarfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        //public IEmployeeRepository EmployeeRepository { get; set; }
        //public IDepartmentRepository DepartmentRepository { get; set; }
        IGerericRepository<T> Repository<T>() where T : ModelBase;

        Task<int> Complete();
    }
}
