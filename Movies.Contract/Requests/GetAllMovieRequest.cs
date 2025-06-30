using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contract.Requests
{
    public class GetAllMovieRequest
    {
        public required string? Title { get; init; }
        public required int? YearOfRelease { get; init; }
    }
}
