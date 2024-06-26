using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using ExpensesApi.Services;
using ExpensesApi.Test.Helpers;
using ExpensesApi.Test.Providers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpensesApi.Test.Services
{
    public class BudgetServicesTest
    {
        private Mock<ExpensesContext> _mockContext;
        private Mock<DbSet<Budget>> _mockDbSet;
        private BudgetServices _service;

        public BudgetServicesTest()
        {

            var data = GetListBudgets().AsQueryable();

            _mockDbSet = new Mock<DbSet<Budget>>();
            _mockDbSet.As<IAsyncEnumerable<Budget>>()
                      .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                      .Returns(new TestDbAsyncEnumerator<Budget>(data.GetEnumerator()));

            _mockDbSet.As<IQueryable<Budget>>()
                      .Setup(m => m.Provider)
                      .Returns(new TestDbAsyncQueryProvider<Budget>(data.Provider));

            _mockDbSet.As<IQueryable<Budget>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Budget>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Budget>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext = new Mock<ExpensesContext>();
            _mockContext.Setup(c => c.Budgets).Returns(_mockDbSet.Object);

            _service = new BudgetServices(_mockContext.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllBudgets()
        {
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ReturnsBudgets()
        {
            var budgetId = 1;
            var budget = GetListBudgets().FirstOrDefault(u => u.BudgetId == budgetId);

            _mockDbSet.Setup(m => m.FindAsync(budgetId))
                      .ReturnsAsync(budget);

            var result = await _service.GetById(budgetId);
            Assert.NotNull(result);
            Assert.Equal(budgetId, result.BudgetId);
        }

        [Fact]
        public async Task Create_AddBudget()
        {
            var budget = new Budget
            {
                BudgetId = 2,
                Name = "budget2",
                Amount = 600.00m,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            await _service.Create(budget);
            _mockDbSet.Verify(m => m.AddAsync(budget, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_SavesChangesBudgets()
        {
            await _service.Update();
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_RemoveBudget()
        {
            var budget = new Budget
            {
                BudgetId = 2,
                Name = "budget2",
                Amount = 600.00m,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now
            };

            await _service.Delete(budget);
            _mockDbSet.Verify(m => m.Remove(budget), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<Budget> GetListBudgets()
        {
            List<Budget> budgets = new()
            {
                new Budget
                {
                    BudgetId = 1,
                    Name = "budget1",
                    Amount = 599.00m,
                    EndDate = DateTime.Now,
                    StartDate = DateTime.Now
                },
                new Budget
                {
                    BudgetId = 2,
                    Name = "budget2",
                    Amount = 600.00m,
                    EndDate = DateTime.Now,
                    StartDate = DateTime.Now
                }
             };
            return budgets;
        }
    }
}
