using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> items, string orderByQueryString) where T : class
        {
            IQueryable<T> result;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                result = items;
            }
            else
            {
                string[] orderParams = orderByQueryString.Trim().Split(',');
                PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var orderQueryBuilder = new StringBuilder();

                foreach (string param in orderParams)
                {
                    if (string.IsNullOrWhiteSpace(param))
                    {
                        continue;
                    }

                    string propertyFromQueryName = param.Split(" ")[0];
                    PropertyInfo? objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                    if (objectProperty == null)
                    {
                        continue;
                    }

                    string direction = param.EndsWith(" desc") ? "descending" : "ascending";
                    orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
                }

                string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

                if (string.IsNullOrWhiteSpace(orderQuery))
                {
                    result = items;
                }
                else
                {
                    result = items.OrderBy(orderQuery);
                }
            }

            return result;
        }
    }
}
