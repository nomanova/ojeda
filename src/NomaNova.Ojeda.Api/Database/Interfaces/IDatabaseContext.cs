using Microsoft.AspNetCore.Builder;

namespace NomaNova.Ojeda.Api.Database.Interfaces
{
    public interface IDatabaseContext
    {
        void EnsureSeeded(IApplicationBuilder app);
    }
}