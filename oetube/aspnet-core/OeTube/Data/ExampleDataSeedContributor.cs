using OeTube.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Data
{
    public class ExampleDataSeedContributor:IDataSeedContributor,ITransientDependency
    {
        private readonly IRepository<Example, Guid> _exampleRepository;

        public ExampleDataSeedContributor(IRepository<Example,Guid> exampleRepository)
        {
            _exampleRepository = exampleRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if(await _exampleRepository.GetCountAsync() <= 0)
            {
                await _exampleRepository.InsertAsync(new Example
                {
                    Name="First Example",
                    Description="Seed Data"
                },true);
                await _exampleRepository.InsertAsync(new Example
                {
                    Name = "Second Example",
                    Description = "Seed Data"
                },true);
            }
        }
    }
}
