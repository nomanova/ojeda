using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NomaNova.Ojeda.Api.Options
{
    public static class OptionExtensions
    {
        public static void ConfigureAndValidate<T>(
            this IServiceCollection serviceCollection, 
            string sectionName, 
            IConfiguration configuration) where T : class, new()
        {
            serviceCollection.Configure<T>(configuration.GetSection(sectionName));

            using var scope = serviceCollection.BuildServiceProvider().CreateScope();
            
            var options = scope.ServiceProvider.GetRequiredService<IOptions<T>>();
            var optionsValue = options.Value;
            var configErrors = ValidationErrors(optionsValue).ToArray();
            if (!configErrors.Any())
            {
                return;
            }

            var aggregatedErrors = string.Join(",", configErrors);
            var count = configErrors.Length;
            var configType = typeof(T).FullName;
            
            throw new ApplicationException(
                $"{configType} configuration has {count} error(s): {aggregatedErrors}");
        }
        
        public static T GetSettings<T>(this IConfiguration configuration, string sectionName)
        {
            return configuration
                .GetSection(sectionName)
                .Get<T>();
        }
        
        private static IEnumerable<string> ValidationErrors(object obj)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);
            foreach (var validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }
    }
}