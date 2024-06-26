using ExpensesApi.Models.Context;
using ExpensesApi.Models.Entities;
using ExpensesApi.Services;
using ExpensesApi.Test.Helpers;
using ExpensesApi.Test.Providers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpensesApi.Test.Services
{
    public class UserAccountServicesTest
    {
        private Mock<ExpensesContext> _mockContext;
        private Mock<DbSet<UserAccount>> _mockDbSet;
        private UserAccountServices _service;

        public UserAccountServicesTest()
        {

            var data = GetListUserAccounts().AsQueryable();

            _mockDbSet = new Mock<DbSet<UserAccount>>();
            _mockDbSet.As<IAsyncEnumerable<UserAccount>>()
                      .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                      .Returns(new TestDbAsyncEnumerator<UserAccount>(data.GetEnumerator()));

            _mockDbSet.As<IQueryable<UserAccount>>()
                      .Setup(m => m.Provider)
                      .Returns(new TestDbAsyncQueryProvider<UserAccount>(data.Provider));

            _mockDbSet.As<IQueryable<UserAccount>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockDbSet.As<IQueryable<UserAccount>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockDbSet.As<IQueryable<UserAccount>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext = new Mock<ExpensesContext>();
            _mockContext.Setup(c => c.UserAccounts).Returns(_mockDbSet.Object);

            _service = new UserAccountServices(_mockContext.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllUserAccounts()
        {
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ReturnsUserAccount()
        {
            var userId = 1;
            var userAccount = GetListUserAccounts().FirstOrDefault(u => u.UserId == userId);

            _mockDbSet.Setup(m => m.FindAsync(userId))
                      .ReturnsAsync(userAccount);

            var result = await _service.GetById(userId);
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task Create_AddsUserAccount()
        {
            var newUserAccount = new UserAccount
            {
                UserId = 3,
                UserName = "user3",
                Email = "user3@example.com",
                PasswordHash = "user3",
                FirstName = "user3",
                LastName = "user3",
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsAdmin = false
            };

            await _service.Create(newUserAccount);
            _mockDbSet.Verify(m => m.AddAsync(newUserAccount, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_SavesChangesUserAccount()
        {
            await _service.Update();
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_RemovesUserAccount()
        {
            var userAccount = new UserAccount
            {
                UserId = 1,
                UserName = "user1",
                Email = "user1@example.com",
                PasswordHash = "user1",
                FirstName = "user1",
                LastName = "user1",
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                IsAdmin = false
            };

            await _service.Delete(userAccount);
            _mockDbSet.Verify(m => m.Remove(userAccount), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<UserAccount> GetListUserAccounts()
        {
            List<UserAccount> userAccounts = new()
            {
                new UserAccount
                {
                    UserId = 1,
                    UserName = "user1",
                    Email = "user1@example.com",
                    PasswordHash = "user1",
                    FirstName = "user1",
                    LastName = "user1",
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsAdmin = false
                },
                new UserAccount
                {
                    UserId = 2,
                    UserName = "user2",
                    Email = "user2@example.com",
                    PasswordHash = "user2",
                    FirstName = "user2",
                    LastName = "user2",
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsAdmin = true
                }
             };

            return userAccounts;
        }
    }
}
