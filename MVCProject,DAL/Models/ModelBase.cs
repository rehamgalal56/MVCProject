using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject_DAL.Models
{
    public class ModelBase 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required !")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        //[MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string Name { get; set; }
    }
}
