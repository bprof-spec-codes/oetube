using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Volo.Abp.Application.Services;

namespace OeTube.Swagger
{
    public class OetubeFirstComparer : IComparer<KeyValuePair<string, OpenApiPathItem>>
    {
        private static readonly HashSet<string> AppServices = Assembly.GetExecutingAssembly().GetTypes()
                                                         .Where(t => t.GetInterface(typeof(IApplicationService).Name) is not null)
                                                         .Select(t => t.Name.Replace("AppService", "")).ToHashSet();

        private bool IsAppService(KeyValuePair<string, OpenApiPathItem> path)
        {
            return path.Value.Operations.SelectMany(o => o.Value.Tags).Any(t => AppServices.Contains(t.Name));
        }

        public int Compare(KeyValuePair<string, OpenApiPathItem> x, KeyValuePair<string, OpenApiPathItem> y)
        {
            var xIsAppService = IsAppService(x);
            var yIsAppService = IsAppService(y);

            if (xIsAppService && yIsAppService)
            {
                return x.Key.CompareTo(y.Key);
            }
            else if (xIsAppService)
            {
                return -1;
            }
            else if (yIsAppService)
            {
                return 1;
            }
            else
            {
                return x.Key.CompareTo(y.Key);
            }
        }
    }

    public class ControllerCustomOrderFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.OrderBy(p => p, new OetubeFirstComparer()).ToList();
            swaggerDoc.Paths.Clear();
            foreach (var item in paths)
            {
                swaggerDoc.Paths.Add(item.Key, item.Value);
            }
        }
    }
}