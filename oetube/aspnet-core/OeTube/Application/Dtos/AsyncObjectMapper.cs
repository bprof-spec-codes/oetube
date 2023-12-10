using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos
{
    public interface IAsyncObjectMapper<in TSource, TDestination> : IObjectMapper<TSource, TDestination>
    {
        public Task<TDestination> MapAsync(TSource source);

        public Task<TDestination> MapAsync(TSource source, TDestination destination);
    }

    public abstract class AsyncObjectMapper<TSource, TDestination> : IAsyncObjectMapper<TSource, TDestination>
    {
        public TDestination Map(TSource source)
        {
            return MapAsync(source).Result;
        }

        public TDestination Map(TSource source, TDestination destination)
        {
            return MapAsync(source, destination).Result;
        }

        public abstract Task<TDestination> MapAsync(TSource source);

        public abstract Task<TDestination> MapAsync(TSource source, TDestination destination);
    }

    public abstract class AsyncNewDestinationObjectMapper<TSource, TDestination> : AsyncObjectMapper<TSource, TDestination>
     where TDestination : new()
    {
        public override async Task<TDestination> MapAsync(TSource source)
        {
            return await MapAsync(source, new TDestination());
        }
    }
}