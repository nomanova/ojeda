using System.Collections.Generic;

namespace NomaNova.Ojeda.Models.Shared
{
    public class PaginatedListDto<T>
    {
        public int TotalCount { get; set; }

        public int PageNumber { get; set; }
        
        public int TotalPages { get; set; }
        
        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public IList<T> Items { get; set; }
    }
}