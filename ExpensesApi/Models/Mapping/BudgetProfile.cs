using AutoMapper;
using ExpensesApi.Models.Dtos;
using ExpensesApi.Models.Entities;

namespace ExpensesApi.Models.Mapping
{
    public class BudgetProfile : Profile
    {
        /// <summary>
        /// Mapping Entry
        /// </summary>
        public BudgetProfile()
        {
            CreateMap<ExpenseDto, Budget>().
               ForMember(d => d.BudgetId, o => o.MapFrom(u => u.Id));

            CreateMap<Budget, ExpenseDto>().
               ForMember(d => d.Id, o => o.MapFrom(u => u.BudgetId));
        }          
    }
}
