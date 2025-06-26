using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class Movie
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required int YearOfRelease { get; init; }
        public required List<string> Generes { get; init; } = new();
    }
}
