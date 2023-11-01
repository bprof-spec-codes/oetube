using OeTube.Application.Services.KeyConverters;

namespace OeTube.Application.Services.GuidBase64Converter
{
    public class TrivialKeyConverter<T> : IKeyConverter<T, T>
    {
        public T InnerToOuterKey(T key)
        {
            return key;
        }

        public T OuterToInnerKey(T outerKey)
        {
            return outerKey;
        }

        object IKeyConverter.InnerToOuterKey(object inner)
        {
            return inner;
        }

        object IKeyConverter.OuterToInnerKey(object outer)
        {
            return outer;
        }
    }
}