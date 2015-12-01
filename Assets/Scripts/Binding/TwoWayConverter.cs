using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class TwoWayConverter<TSource,TTarget> : IValueConverter
    {
        public abstract TTarget Convert(TSource source, CultureInfo culture);

        public abstract bool CanConvert(TSource source, CultureInfo culture);

        public abstract TSource ConvertBack(TTarget target, CultureInfo culture);

        public abstract bool CanConvertBack(TTarget target, CultureInfo culture);

        object IValueConverter.Convert(object source, CultureInfo culture)
        {
            return Convert((TSource)source, culture);
        }

        bool IValueConverter.CanConvert(object source, CultureInfo culture)
        {
            return CanConvert((TSource)source, culture);
        }

        object IValueConverter.ConvertBack(object target, CultureInfo culture)
        {
            return ConvertBack((TTarget)target, culture);
        }

        bool IValueConverter.CanConvertBack(object target, CultureInfo culture)
        {
            return CanConvertBack((TTarget)target, culture);
        }
    }
}
