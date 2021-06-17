using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChurchManager.Infrastructure.Persistence.Extensions
{
    public static class DatabaseExtensions
    {
        public static CustomTypeSqlQuery<T> SqlQuery<T>(
            this DatabaseFacade database, 
            string sqlQuery,
            params DbParameter[] sqlParameters) where T : class
        {
            return new()
            {
                DatabaseFacade = database,
                SqlQuery = sqlQuery,
                SqlParameters = sqlParameters
            };
        }
    }
}
