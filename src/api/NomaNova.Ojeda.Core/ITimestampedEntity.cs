using System;

namespace NomaNova.Ojeda.Core
{
    public interface ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}