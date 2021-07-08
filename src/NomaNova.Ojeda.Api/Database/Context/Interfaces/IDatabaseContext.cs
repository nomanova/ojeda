using Microsoft.AspNetCore.Builder;

namespace NomaNova.Ojeda.Api.Database.Context.Interfaces
{
    public interface IDatabaseContext
    {
        void EnsureSeeded(IApplicationBuilder app);
    }
}