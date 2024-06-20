using AutoMapper;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;

namespace ExpensesApi.Models.Mapping
{
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Mapping Entry
        /// </summary>
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().
               ForMember(d => d.CategoryId, o => o.MapFrom(u => u.Id));

            CreateMap<Category, CategoryDto>().
               ForMember(d => d.Id, o => o.MapFrom(u => u.CategoryId));
        }
    }
}
