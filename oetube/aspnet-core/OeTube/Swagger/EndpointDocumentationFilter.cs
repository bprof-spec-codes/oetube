using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace OeTube.Swagger
{
    public class EndpointDocumentationFilter : IDocumentFilter
    {
        private readonly static IReadOnlyDictionary<OperationType, int> MethodPrecedences = new Dictionary<OperationType, int>() {
        {OperationType.Get,0 },
        {OperationType.Post,1 },
        {OperationType.Put,2 },
        {OperationType.Delete,3 },
        {OperationType.Options,4 },
        };
        private static int? _maxMethodPrecedence;
        private static int MaxMethodPrecendence
        {
            get
            {
                if(_maxMethodPrecedence is null)
                {
                    _maxMethodPrecedence = MethodPrecedences.Max(o => o.Value)+1;
                }
                return _maxMethodPrecedence.Value;
            }
        }
        private static int GetPrecedence(OperationType op)
        {
            int opPrec = MaxMethodPrecendence;
            if (MethodPrecedences.ContainsKey(op))
            {
                opPrec = MethodPrecedences[op];
            }
            return opPrec;
        }
        private static IComparer<OperationType> MethodComparer = Comparer<OperationType>.Create((x, y) => GetPrecedence(x).CompareTo(GetPrecedence(y)));

        SortedDictionary<string, SortedDictionary<OperationType, List<EndpointDocumentation>>> docs = new();

 
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
           
            foreach (var item in swaggerDoc.Paths)
            {
                if (AppServiceUtility.IsAppService(item))
                {
                    string route = item.Key;
                   
                    foreach (var op in item.Value.Operations)
                    {
                        string description = op.Value.Description;
                        string name = op.Value.Tags.First().Name;
                        if (!docs.ContainsKey(name))
                        {
                            docs.Add(name,new(MethodComparer));
                        }
                        if (!docs[name].ContainsKey(op.Key))
                        {
                            docs[name].Add(op.Key,new ());
                        }

                        docs[name][op.Key].Add(new EndpointDocumentation(route, description));
                    }
                }
            }
            WriteEndpointMd();
        }
        private void WriteEndpointMd()
        {
            string Indent(int count)
            {
                return new string('\t', count);
            }
            StringBuilder sb = new StringBuilder();
            foreach (var controller in docs)
            {
                sb.AppendLine($"+ #### /{controller.Key} ####");
                foreach (var method in controller.Value)
                {
                    sb.AppendLine($"{Indent(1)}+ **{method.Key}**");
                    foreach (var endpoint in method.Value)
                    {
                        if (endpoint.Description is null)
                        {
                            sb.AppendLine($"{Indent(2)}+ **{endpoint.Route}**");
                        }
                        else
                        {
                            sb.AppendLine($"{Indent(2)}+ **{endpoint.Route}:** *{endpoint.Description}*");
                        }
                    }
                }
            };
            File.WriteAllText("endpoint.md", sb.ToString());
        }
    }
}