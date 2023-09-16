using OeTube.Entities;
using OeTube.Services.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OeTube.Services
{
    public class ExampleService:ApplicationService
    {
        private readonly IRepository<Example, Guid> _exampleRepository;

        public ExampleService(IRepository<Example, Guid> exampleRepository)
        {
            _exampleRepository = exampleRepository;
        }
        public async Task<List<ExampleDto>> GetListAsync()
        {
            var items = await _exampleRepository.GetListAsync();
            return items
                .Select(item => new ExampleDto
                {
                    Id = item.Id,
                    Name=item.Name,
                    Description = item.Description
                }).ToList();
        }
        public async Task<ExampleDto> CreateAsync(string name, string description)
        {
            var example = await _exampleRepository.InsertAsync(new Example
            {
                Name=name,
                Description=description
            });
            return new ExampleDto
            {
                Id = example.Id,
                Name = example.Name,
                Description = example.Description
            };
        }
        public async Task DeleteAsync(Guid id)
        {
            await _exampleRepository.DeleteAsync(id);
        }
    }
}
