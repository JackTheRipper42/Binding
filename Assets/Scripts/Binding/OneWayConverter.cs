using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class OneWayConverter<TSource, TTarget> : IValueConverter
    {
        public abstract TTarget Convert(TSource source, CultureInfo culture);

        public abstract bool CanConvert(TSource source, CultureInfo culture);

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
            throw new NotSupportedException(string.Format(
                "The '{0}' convert does not support conversion back to source.",
                GetType().Name));
        }

        bool IValueConverter.CanConvertBack(object target, CultureInfo culture)
        {
            return false;
        }
    }
}
