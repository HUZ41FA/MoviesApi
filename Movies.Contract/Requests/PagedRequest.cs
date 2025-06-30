using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contract.Requests
{
    public class PagedRequest
    {
        public required int PageNumber { get; init; } = 1;
        public required int PageSize { get; init; } = 10;
    }
}
