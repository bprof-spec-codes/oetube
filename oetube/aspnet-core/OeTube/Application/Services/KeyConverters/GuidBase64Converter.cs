using OeTube.Application.Services.KeyConverters;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services.GuidBase64Converter
{
    public class GuidBase64Converter : IKeyConverter<Guid, string>, ITransientDependency
    {
        public Guid OuterToInnerKey(string outerKey)
        {
            outerKey = outerKey.Replace("-", "/").Replace("_", "+") + "==";
            return new Guid(Convert.FromBase64String(outerKey));
        }

        public string InnerToOuterKey(Guid key)
        {
            return Convert.ToBase64String(key.ToByteArray()).Replace('/', '-').Replace('+', '_')[..22];
        }

        public object OuterToInnerKey(object outer)
        {
            if (outer is string outerKey)
            {
                return OuterToInnerKey(outerKey);
            }
            else throw new InvalidCastException(outer.GetType().Name);
        }

        public object InnerToOuterKey(object inner)
        {
            if (inner is Guid innerKey)
            {
                return InnerToOuterKey(innerKey);
            }
            else throw new InvalidCastException(inner.GetType().Name);
        }
    }
}