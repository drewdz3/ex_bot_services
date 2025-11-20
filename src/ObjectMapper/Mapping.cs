using System.Reflection;

namespace ObjectMapper
{
    public class Mapping
    {
        #region Fields

        Func<object, object> _Conversion;

        #endregion Fields

        #region Constructors

        public Mapping()
        {
        }

        public Mapping(PropertyInfo sourceProperty, PropertyInfo targetProperty)
        {
            _SourceProperty = sourceProperty;
            _TargetProperty = targetProperty;
            _Conversion = (o) => o;
        }

        public Mapping(PropertyInfo sourceProperty, PropertyInfo targetProperty, Func<object, object> conversion) 
            : this(sourceProperty, targetProperty)
        {
            if (conversion == null) throw new ArgumentNullException("Conversion function cannot be null");
            _Conversion = conversion;
        }

        #endregion Constructors

        #region Properties

        PropertyInfo _SourceProperty;
        public PropertyInfo SourceProperty
        {
            get { return _SourceProperty; }
            set { _SourceProperty = value; }
        }

        PropertyInfo _TargetProperty;
        public PropertyInfo TargetProperty
        {
            get { return _TargetProperty; }
            set { _TargetProperty = value; }
        }

        #endregion Properties

        #region Conversion

        public void Convert(object from, object to)
        {
            if (from == null) throw new ArgumentNullException("Source object is null");
            if (to == null) throw new ArgumentNullException("Destination object is null");
            //  get the value of the source property
            var fromValue = _SourceProperty.GetValue(from);
            if (fromValue == null) throw new ArgumentNullException("Source property (${sourceField}) not found");
            //  convert the value
            var toValue = _Conversion?.Invoke(fromValue);
            //  apply it
            _TargetProperty.SetValue(to, toValue);
        }

        public Mapping WithConversion(Func<object, object> conversion)
        {
            _Conversion = conversion;
            return this;
        }

        #endregion Conversion
    }
}
