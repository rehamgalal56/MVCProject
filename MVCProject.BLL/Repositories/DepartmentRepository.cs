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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
       
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

}
