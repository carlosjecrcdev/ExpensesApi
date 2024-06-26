using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using ExpensesApi.Services;
using ExpensesApi.Test.Helpers;
using ExpensesApi.Test.Providers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpensesApi.Test.Services
{
    public class CategoryServicesTest
    {
        private Mock<ExpensesContext> _mockContext;
        private Mock<DbSet<Category>> _mockDbSet;
        private CategoryServices _service;

        public CategoryServicesTest()
        {

            var data = GetListCategory().AsQueryable();

            _mockDbSet = new Mock<DbSet<Category>>();
            _mockDbSet.As<IAsyncEnumerable<Category>>()
                      .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                      .Returns(new TestDbAsyncEnumerator<Category>(data.GetEnumerator()));

            _mockDbSet.As<IQueryable<Category>>()
                      .Setup(m => m.Provider)
                      .Returns(new TestDbAsyncQueryProvider<Category>(data.Provider));

            _mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext = new Mock<ExpensesContext>();
            _mockContext.Setup(c => c.Categories).Returns(_mockDbSet.Object);

            _service = new CategoryServices(_mockContext.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllCategory()
        {
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ReturnsCategory()
        {
            var categoryId = 1;
            var category = GetListCategory().FirstOrDefault(u => u.CategoryId == categoryId);

            _mockDbSet.Setup(m => m.FindAsync(categoryId))
                      .ReturnsAsync(category);

            var result = await _service.GetById(categoryId);
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.CategoryId);
        }

        [Fact]
        public async Task Create_AddCategory()
        {
            var category = new Category
            {
                CategoryId = 3,
                Name = "Test",
                Description = "Test"
            };

            await _service.Create(category);
            _mockDbSet.Verify(m => m.AddAsync(category, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_SavesChangesCategory()
        {
            await _service.Update();
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_RemoveCategory()
        {
            var category = new Category
            {
                CategoryId = 3,
                Name = "Test",
                Description = "Test"
            };

            await _service.Delete(category);
            _mockDbSet.Verify(m => m.Remove(category), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<Category> GetListCategory()
        {
            List<Category> category = new()
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Test",
                    Description = "Test"
                },
                new Category
                {
                    CategoryId = 1,
                    Name = "Test",
                    Description = "Test"
                }
             };
            return category;
        }
    }
}
