using System;

namespace NomaNova.Ojeda.Core.Helpers.Interfaces
{
    public interface ITimeKeeper
    {
        DateTime UtcNow { get; }
    }
}