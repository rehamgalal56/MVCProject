using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject_DAL.Models
{
    public class Department :ModelBase
    {
        
       [Required (ErrorMessage ="Code is Required !!")]
        public string Code { get; set; }
        //[Required(ErrorMessage = "Name is Required !!")]
        //public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
