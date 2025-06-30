using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore
{
    public class PaginatedList<T> : List<T>
    {
        public const string HttpHeaderKey = "X-Pagination";

        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> items,int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public string ToHttpHeaderValue()
        {
            var metadata = new
            {
                TotalCount,
                PageSize,
                CurrentPage,
                TotalPages,
                HasNextPage,
                HasPreviousPage
            };

            return JsonSerializer.Serialize(metadata);
        }
    }
}
