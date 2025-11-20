namespace ObjectMapper
{
    public interface IConversionSet<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        #region Properties

        Type SourceType { get; }

        Type TargetType { get; }

        #endregion Properties

        #region Operations

        IConversionSet<TSource, TTarget> WithConversion(string sourceProperty, string targetProperty, Func<object, object> conversion);

        TTarget Map(TSource source);

        IConversionSet<TTarget, TSource> AndBack();

        #endregion Operations
    }
}
