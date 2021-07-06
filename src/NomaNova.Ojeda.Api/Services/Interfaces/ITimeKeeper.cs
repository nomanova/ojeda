using System;

namespace NomaNova.Ojeda.Api.Services.Interfaces
{
    public interface ITimeKeeper
    {
        DateTime UtcNow { get; }
    }
}