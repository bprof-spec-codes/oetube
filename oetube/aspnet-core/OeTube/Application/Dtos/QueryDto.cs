using OeTube.Domain.Repositories.QueryArgs;
using System.ComponentModel.DataAnnotations;

namespace OeTube.Application.Dtos
{
    public class QueryDto : IQueryArgs
    {
        public ItemPerPage ItemPerPage { get; set; } = ItemPerPage.P10;
        [Range(0, int.MaxValue)]
        public int Page { get; set; } = 0;
        public string? Sorting { get; set; }
    }
}