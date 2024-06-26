using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using ExpensesApi.Services;
using ExpensesApi.Test.Helpers;
using ExpensesApi.Test.Providers;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace ExpensesApi.Test.Services
{
    public class ExpenseServicesTest
    {
        private Mock<ExpensesContext> _mockContext;
        private Mock<DbSet<Expense>> _mockDbSet;
        private ExpenseServices _service;

        public ExpenseServicesTest()
        {

            var data = GetListBudgets().AsQueryable();

            _mockDbSet = new Mock<DbSet<Expense>>();
            _mockDbSet.As<IAsyncEnumerable<Expense>>()
                      .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                      .Returns(new TestDbAsyncEnumerator<Expense>(data.GetEnumerator()));

            _mockDbSet.As<IQueryable<Expense>>()
                      .Setup(m => m.Provider)
                      .Returns(new TestDbAsyncQueryProvider<Expense>(data.Provider));

            _mockDbSet.As<IQueryable<Expense>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Expense>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Expense>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext = new Mock<ExpensesContext>();
            _mockContext.Setup(c => c.Expenses).Returns(_mockDbSet.Object);

            _service = new ExpenseServices(_mockContext.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllExpenses()
        {
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ReturnsExpense()
        {
            var expenseId = 1;
            var expense = GetListBudgets().FirstOrDefault(u => u.ExpenseId == expenseId);

            _mockDbSet.Setup(m => m.FindAsync(expenseId))
                      .ReturnsAsync(expense);

            var result = await _service.GetById(expenseId);
            Assert.NotNull(result);
            Assert.Equal(expenseId, result.ExpenseId);
        }

        [Fact]
        public async Task Create_AddExpense()
        {
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "TestExpense1",
                CategoryId = 1,
                Amount = 12.00m,
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            await _service.Create(expense);
            _mockDbSet.Verify(m => m.AddAsync(expense, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_SavesChangesExpense()
        {
            await _service.Update();
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_RemoveExpense()
        {
            var expense = new Expense
            {
                ExpenseId = 1,
                Description = "TestExpense1",
                CategoryId = 1,
                Amount = 12.00m,
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            await _service.Delete(expense);
            _mockDbSet.Verify(m => m.Remove(expense), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<Expense> GetListBudgets()
        {
            List<Expense> expense = new()
            {
                new Expense
                {
                    ExpenseId = 1,
                    Description = "TestExpense1",
                    CategoryId = 1,
                    Amount = 12.00m,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now

                },
                new Expense
                {   
                    ExpenseId = 2,
                    Description = "TestExpense2",
                    CategoryId = 1,
                    Amount = 12.00m,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                }
             };
            return expense;
        }
    }
}
