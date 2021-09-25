using System;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Utils.Services
{
    public class TimeKeeper : ITimeKeeper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}