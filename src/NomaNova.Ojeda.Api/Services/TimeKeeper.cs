using System;
using NomaNova.Ojeda.Api.Services.Interfaces;

namespace NomaNova.Ojeda.Api.Services
{
    public class TimeKeeper : ITimeKeeper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}