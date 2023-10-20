using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileContainers
{
    [ExposeServices(typeof(IFileContainerFactory))]
    public class FileContainerFactory : IFileContainerFactory,ITransientDependency
    {
        private readonly IBlobContainerFactory _containerFactory;
        private readonly IBlobFilePathCalculator _calculator;
        private readonly IBlobContainerConfigurationProvider _provider;

        public FileContainerFactory(IBlobContainerFactory containerFactory,
                                    IBlobFilePathCalculator calculator,
                                    IBlobContainerConfigurationProvider provider)
        {
            this._containerFactory = containerFactory;
            this._calculator = calculator;
            this._provider = provider;
        }
        public IFileContainer Create(string containerName)
        {
            return new FileContainer(containerName, _containerFactory, _calculator, _provider);
        }
    }
}
