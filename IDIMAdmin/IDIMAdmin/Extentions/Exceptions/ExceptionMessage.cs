using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace IDIMAdmin.Extentions.Exceptions
{
	public static class ExceptionMessage
    {
        public static string Message(this Exception exception)
        {
            string message = exception.Message;

            // DbEntityValidationException
            // SqlException
            // ArgumentException
            // ArgumentNullException
            // DbUpdateConcurrencyException

            if (exception is DbUpdateException)
            {
                var exceptionInner = exception.InnerException?.InnerException;
                if (exceptionInner is SqlException)
                {
                    var sqlException = (SqlException) exceptionInner;
                    if (sqlException.Number == 547)
                    {
                        message = "Deleted data conflicted with reference table.";
                    }
                }
            }

            return message;
        }
    }
}