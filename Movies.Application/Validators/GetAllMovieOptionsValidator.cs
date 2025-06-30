using FluentValidation;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class GetAllMovieOptionsValidator : AbstractValidator<GetAllMovieOptions>
    {
        public GetAllMovieOptionsValidator() 
        {
            RuleFor(x => x.YearOfRelease)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);
                
        
        }
    }
}
