using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.Url
{

    public interface IUrlService
    {
        string BaseUrl { get; }

        string GetUrl(string template, params RouteTemplateParameter[] parameters);

        string? GetUrl<T>(string methodName, params RouteTemplateParameter[] parameters);
    }

    public class UrlService : ITransientDependency, IUrlService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UrlService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string BaseUrl => $"{contextAccessor?.HttpContext?.Request.Scheme}://{contextAccessor?.HttpContext?.Request.Host}";

        public string GetUrl(string template, params RouteTemplateParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                template = template.Replace("{" + item.Name + "}", item.Value.ToString());
            }
            return BaseUrl + "/" + template;
        }

        public string GetUrl<T>(string methodName, params RouteTemplateParameter[] parameters)
        {
            var type = typeof(T);
            var attribute = type.GetMethod(methodName)?.GetCustomAttribute<HttpMethodAttribute>();
            if (attribute is null || attribute.Template is null)
            {
                throw new ArgumentException(typeof(T).Name);
            }

            return GetUrl(attribute.Template, parameters);
        }
    }
}