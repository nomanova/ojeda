using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core;

namespace NomaNova.Ojeda.Api.Tests.Helpers
{
    public static class DatabaseHelper
    {
        public static async Task<T> Get<T>(DbContext context, string id) where T : BaseEntity
        {
            var item = await context.Set<T>()
                .Where(e => e.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (item != null)
            {
                // Ensure latest version is fetched,
                // even when different scopes are used.
                await context.Entry(item).ReloadAsync();
            }

            return item;
        }

        public static async Task Add<T>(DbContext context, T item) where T : class
        {
            context.Set<T>().Add(item);
            await context.SaveChangesAsync();
        }
    }
}