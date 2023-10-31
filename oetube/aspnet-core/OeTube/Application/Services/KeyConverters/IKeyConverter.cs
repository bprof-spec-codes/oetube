namespace OeTube.Application.Services.KeyConverters
{
    public interface IKeyConverter
    {
        object OuterToInnerKey(object outer);

        object InnerToOuterKey(object inner);
    }

    public interface IKeyConverter<TInner, TOuter> : IKeyConverter
    {
        TInner OuterToInnerKey(TOuter outer);

        TOuter InnerToOuterKey(TInner inner);
    }
}