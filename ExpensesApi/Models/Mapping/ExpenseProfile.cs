using AutoMapper;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;

namespace ExpensesApi.Models.Mapping
{
    public class ExpenseProfile : Profile
    {
        /// <summary>
        /// Mapping Entry
        /// </summary>
        public ExpenseProfile()
        {
            CreateMap<ExpenseDto, Expense>().
                   ForMember(d => d.CategoryId, o => o.MapFrom(u => u.CatId)).
                   ForMember(d => d.ExpenseId, o => o.MapFrom(u => u.Id));

            CreateMap<Expense, ExpenseDto>().
               ForMember(d => d.CatId, o => o.MapFrom(u => u.CategoryId)).
               ForMember(d => d.Id, o => o.MapFrom(u => u.ExpenseId));

        }
    }
}
