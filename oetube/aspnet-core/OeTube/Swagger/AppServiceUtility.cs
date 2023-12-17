using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Volo.Abp.Application.Services;

namespace OeTube.Swagger
{
    public static class AppServiceUtility
    {
        public static readonly IReadOnlyDictionary<string, Type> AppServices = Assembly.GetExecutingAssembly().GetTypes()
                                                        .Where(t => t.GetInterface(typeof(IApplicationService).Name) is not null)
                                                        .ToDictionary(t => t.Name.Replace("AppService", ""), t => t);
        public static bool IsAppService(KeyValuePair<string, OpenApiPathItem> path)
        {
            return path.Value.Operations.SelectMany(o => o.Value.Tags).Any(t => AppServices.ContainsKey(t.Name));
        }

    }

    public class EndpointDocumentation
    {
        public EndpointDocumentation(string route, string? description)
        {
            Route = route;
            Description = description;
        }

        public string Route { get; }
        public string? Description { get; }
    }
}