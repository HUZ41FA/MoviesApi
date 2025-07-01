using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IHostEnvironment _env;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IHostEnvironment env)
        {
            _provider = provider;
            _env = env;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = _env.ApplicationName,
                        Version = description.ApiVersion.ToString(),
                    }
                    );
            }
        }
    }
}
