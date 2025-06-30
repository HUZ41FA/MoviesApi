using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class GetAllMovieOptions
    {
        public string? Title { get; set; }
        public int? YearOfRelease { get; set; }
        public Guid? UserId { get; set; }
        public string? SortField { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.None;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum SortOrder
    {
        None,
        Ascending,
        Descending
    }
}
