namespace ObjectMapper
{
    public class ConversionSet<TSource, TTarget> : IConversionSet<TSource, TTarget> 
        where TSource : class
        where TTarget : class
    {
        #region Fields

        List<Mapping> _Mappings = new();

        #endregion Fields

        #region Constructors

        public ConversionSet()
        {
            SourceType = typeof(TSource);
            TargetType = typeof(TTarget);
            Initialize();
        }

        #endregion Constructors

        #region Properties

        virtual public Type SourceType { get; protected set; }

        virtual public Type TargetType { get; protected set; }

        #endregion Properties

        #region Initialization

        virtual protected void Initialize()
        {
            //  find all the source public properties
            foreach (var inProperty in SourceType.GetProperties())
            {
                if (!inProperty.CanRead) continue;
                //  find a matching writable property on the destination type
                var outProperty = TargetType.GetProperty(inProperty.Name);
                if ((outProperty == null) || !outProperty.CanWrite) continue;
                //  create a mapping
                _Mappings.Add(new Mapping(inProperty, outProperty));
            }
        }

        #endregion Initialization

        #region Implementation

        public IConversionSet<TSource, TTarget> WithConversion(string sourceProperty, string targetProperty, Func<object, object> conversion)
        {
            //  create new mapping
            var source = typeof(TSource).GetProperty(sourceProperty);
            var target = typeof(TTarget).GetProperty(targetProperty);
            if ((source == null) || (target == null)) throw new ArgumentException("Source or Target property not found");
            //  check if we have a mapping for this member
            var exists = _Mappings.FirstOrDefault(m => m.SourceProperty.Name.Equals(sourceProperty) && m.TargetProperty.Name.Equals(targetProperty));
            //  ensure existing mapping is between the same two properties
            if ((exists != null) && !exists.TargetProperty.Name.Equals(targetProperty))
            {
                _Mappings.Remove(exists);
            }
            if (exists == null)
            {
                //  create a new vanilla mapping 
                exists = new Mapping(source, target);
            }
            //  set the custom mapping
            exists.WithConversion(conversion);
            return this;
        }

        public TTarget Map(TSource source)
        {
            //  iterate all mappings and execute conversions
            var target = Activator.CreateInstance<TTarget>();
            foreach (var mapping in _Mappings)
            {
                mapping.Convert(source, target);
            }
            return target;
        }

        public IConversionSet<TTarget, TSource> AndBack()
        {
            return new ConversionSet<TTarget, TSource>();
        }

        #endregion Implementation
    }
}
