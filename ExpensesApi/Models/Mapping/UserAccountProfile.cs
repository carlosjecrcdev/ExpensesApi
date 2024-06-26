using AutoMapper;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;

namespace ExpensesApi.Models.Mapping
{
    public class UserAccountProfile : Profile
    {
        /// <summary>
        /// Mapping entity
        /// </summary>
        public UserAccountProfile() 
        {
            CreateMap<UserAccountDto, UserAccount >().
                ForMember(d => d.UserId, o => o.MapFrom(u => u.Id)).
                ForMember(d => d.PasswordHash, o => o.MapFrom(u => u.Password)).
                ForMember(d => d.IsAdmin, o => o.MapFrom(u => u.Admin));

            CreateMap<UserAccount, UserAccountDto>().
               ForMember(d => d.Id, o => o.MapFrom(u => u.UserId)).
               ForMember(d => d.Password, o => o.MapFrom(u => u.PasswordHash)).
               ForMember(d => d.Admin, o => o.MapFrom(u => u.IsAdmin));
        }
    }
}
