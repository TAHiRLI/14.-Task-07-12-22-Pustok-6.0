using System;
using System.Collections.Generic;
using System.Linq;

namespace Pustok.Areas.Admin.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            this.AddRange(items);
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPage;

        public static PaginatedList<T> Create(IQueryable<T> query, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(
                query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(), 
                query.Count(),
                pageIndex,
                pageSize);
        }
    }
}
