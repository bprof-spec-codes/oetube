using OeTube.Domain.Repositories.QueryArgs;
using System.ComponentModel.DataAnnotations;

namespace OeTube.Application.Dtos
{
    public class QueryDto : IQueryArgs
    {
        public Pagination? Pagination { get; set; } 
        public string? Sorting { get; set; }
    }
}