using Microsoft.AspNetCore.Http;
using MVCProject_DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MVCProject.PL.ViewModels
{
    //public enum Gender
    //{
    //    [EnumMember(Value = "Male")]
    //    Male = 1,
    //    [EnumMember(Value = "Female")]

    //    Female = 2,
    //}
    //public enum EmpType
    //{
    //    FullTime = 1,
    //    PartTime = 2,
    //}
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required !")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
           , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public String Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmpType EmployeeType { get; set; }

        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }

        public IFormFile Image { get; set; }
        public String ImageName { get; set; }
    }
}
