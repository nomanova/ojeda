using System;

namespace NomaNova.Ojeda.Utils.Services.Interfaces
{
    public interface ITimeKeeper
    {
        DateTime UtcNow { get; }
    }
}