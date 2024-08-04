using AutoMapper;
using MVCProject.PL.ViewModels;
using MVCProject_DAL.Models;

namespace MVCProject.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
