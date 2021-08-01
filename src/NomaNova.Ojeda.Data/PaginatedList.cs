using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NomaNova.Ojeda.Data
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalCount { get; private set; }
        
        public int PageNumber { get; private set; }
        
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IOrderedQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedList<TS> Create<TS>(
            IEnumerable<TS> items, int totalCount, int pageNumber, int pageSize)
        {
            return new PaginatedList<TS>(items, totalCount, pageNumber, pageSize);
        }
    }
}