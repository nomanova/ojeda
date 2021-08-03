using System;
using NomaNova.Ojeda.Core.Helpers.Interfaces;

namespace NomaNova.Ojeda.Core.Helpers
{
    public class TimeKeeper : ITimeKeeper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}