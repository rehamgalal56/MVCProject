using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.BLL.Intarfaces
{
    public interface IEmployeeRepository : IGerericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAddress (string address);  
    }
}
