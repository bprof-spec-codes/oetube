using System.Runtime.CompilerServices;

namespace OeTube.Application.Services.Url
{
    public struct RouteTemplateParameter
    {
        public string Name { get; }
        public object Value { get; }

        public RouteTemplateParameter(object value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            Value = value;
            Name = parameterName ?? string.Empty;
        }
    }
}