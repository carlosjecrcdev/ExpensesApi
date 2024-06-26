
using ExpensesApi.Test.Providers;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Linq.Expressions;

namespace ExpensesApi.Test.Helpers
{  
    public class TestDbAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public IQueryable CreateQuery(Expression expression) => new TestDbAsyncEnumerable<TEntity>(_inner.CreateQuery<TEntity>(expression));

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestDbAsyncEnumerable<TElement>(_inner.CreateQuery<TElement>(expression));

        public object Execute(Expression expression) => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            var result = _inner.Execute<IEnumerable<TResult>>(expression);
            return new TestDbAsyncEnumerable<TResult>(result);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var result = _inner.Execute<TResult>(expression);
            return Task.FromResult(result);
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return ExecuteAsync<TResult>(expression, cancellationToken).Result;
        }
    }
}
