using ExpensesApi.Test.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesApi.Test.Providers
{
    public class TestDbAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryable<T> _inner;

        public TestDbAsyncEnumerable(IEnumerable<T> inner)
        {
            _inner = inner.AsQueryable() ?? throw new ArgumentNullException(nameof(inner));
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new TestDbAsyncEnumerator<T>(_inner.GetEnumerator());

        public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();

        public Type ElementType => _inner.ElementType;

        public Expression Expression => _inner.Expression;

        public IQueryProvider Provider => new TestDbAsyncQueryProvider<T>(_inner.Provider);
    }
}
