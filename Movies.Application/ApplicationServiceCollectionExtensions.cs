﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IRatingRepository, RatingRepository>();
            services.AddSingleton<IRatingService, RatingService>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton); // IApplicationMarker is just a marker to mark this assembly
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            // Although the service is injected as singleton,
            // but the connection is created everytime the CreateConnection
            // method is called. Basically you can say that a singleton masking a transient
            services.AddSingleton<IDatabaseConnectionFactory>(_ => 
                new NpgConnectionFactory(connectionString));
            services.AddSingleton<DatabaseInitializer>();
            return services;
        }
    }
}
