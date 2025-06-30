using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contract.Responses
{
    public class PagedResponse<TResponse>
    {
        public required IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();
        public required int PageSize { get; init; }
        public required int PageNumber { get; init; }
        public required int TotalCount { get; init; }
        public bool HasNextPage => TotalCount > PageSize * PageNumber;
    }
}
