using AutoMapper;
using WebApplication.DAL.Models;
using WebApplication.PL.ViewModels;

namespace WebApplication.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
               
           
        }
    }
}
