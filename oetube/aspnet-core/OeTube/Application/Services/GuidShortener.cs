using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Services
{
    public interface IGuidShortener
    {
        Guid ConvertBackFromBase64Url(string convertedGuid);
        string ConvertToBase64Url(Guid guid);
    }

    public class GuidShortener : IGuidShortener,ITransientDependency
    {
        public string ConvertToBase64Url(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).Replace('/', '-').Replace('+', '_')[..22];
        }
        public Guid ConvertBackFromBase64Url(string convertedGuid)
        {
            convertedGuid = convertedGuid.Replace("-", "/").Replace("_", "+") + "==";
            return new Guid(Convert.FromBase64String(convertedGuid));
        }
    }
}
