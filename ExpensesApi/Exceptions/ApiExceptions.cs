using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Globalization;

namespace ExpensesApi.Exceptions
{
    public class ApiExceptions : Exception
    {
        public ApiExceptions() : base() { }

        public ApiExceptions(string message) : base(message) { }

        public ApiExceptions(string message, params object[] args)
            :base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
