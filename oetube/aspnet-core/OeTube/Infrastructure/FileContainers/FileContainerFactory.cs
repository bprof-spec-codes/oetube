using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FileClassContainers
{

    public class FileContainerFactory : IFileContainerFactory,ITransientDependency
    {
        private readonly IBlobContainerFactory containerFactory;
        private readonly IBlobFilePathCalculator calculator;
        private readonly IBlobContainerConfigurationProvider provider;

        public FileContainerFactory(
                             IBlobContainerFactory containerFactory,
                             IBlobFilePathCalculator calculator,
                             IBlobContainerConfigurationProvider provider)
        {
            this.containerFactory = containerFactory;
            this.calculator = calculator;
            this.provider = provider;
        }

        public IFileContainer Create<TRelatedType>()
        {
            return new FileContainer(typeof(TRelatedType), containerFactory, calculator, provider);

        }

    }
}
