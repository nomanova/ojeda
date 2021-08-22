using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core;
using NomaNova.Ojeda.Data.Options;

namespace NomaNova.Ojeda.Data
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> ExecuteQueryFilter<TEntity>(
            this IQueryable<TEntity> queryable, string query, DatabaseType databaseType = DatabaseType.Postgresql)
            where TEntity : BaseEntity
        {
            // No search query, just return
            if (string.IsNullOrEmpty(query))
            {
                return queryable;
            }
            
            // Find searchable properties
            var properties = typeof(TEntity).GetProperties()
                .Where(p => p.PropertyType == typeof(string) &&
                            p.GetCustomAttributes(typeof(SearchableAttribute), true).FirstOrDefault() != null)
                .Select(x => x.Name).ToList();
            
            // No properties, no filtering
            if (!properties.Any())
            {
                return queryable;
            }
            
            var entity = Expression.Parameter(typeof(TEntity), "entity");
            
            // Get the 'Like' Method from EF.Functions, use ILike with postgres for case-insensitivity
            MethodInfo efLikeMethod;
            
            if (databaseType == DatabaseType.Postgresql)
            {
                efLikeMethod = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike", 
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null, 
                    new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                    null
                );
            }
            else
            {
                efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                    null);
            }

            if (efLikeMethod == null)
            {
                throw new Exception("Cannot find EF 'ILike/Like' method.");
            }

            // Create search pattern
            var pattern = Expression.Constant($"%{query}%", typeof(string));
            
            // Create a search clause for each property
            Expression body = Expression.Constant(false);

            foreach (var propertyName in properties)
            {
                // Get property from our object
                var property = Expression.Property(entity, propertyName);

                // Call method with required arguments
                var expr = Expression.Call(efLikeMethod,
                    Expression.Property(null, typeof(EF), nameof(EF.Functions)), property, pattern);

                // Add to the main request
                body = Expression.OrElse(body, expr);
            }

            // Compose and pass the expression to Where
            var expression = Expression.Lambda<Func<TEntity, bool>>(body, entity);
            
            return queryable.Where(expression);
        }
        
        public static IQueryable<TEntity> ExecuteOrderBy<TEntity>(this IQueryable<TEntity> queryable, string orderBy, bool orderAsc) 
        {
            // No order field, just return
            if (string.IsNullOrEmpty(orderBy))
            {
                return queryable;
            }

            var textInfo = new CultureInfo("en-US",false).TextInfo;
            var normalizedOrderBy = textInfo.ToTitleCase(orderBy);
            
            var type = typeof(TEntity);
            var property = type.GetProperty(normalizedOrderBy);

            // No property found, no ordering
            if (property == null)
            {
                return queryable;
            }

            var command = orderAsc ? "OrderBy" : "OrderByDescending";

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new [] { type, property.PropertyType },
                queryable.Expression, Expression.Quote(orderByExpression));
            
            return queryable.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}