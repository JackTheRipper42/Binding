using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public abstract class OneWayToSourceConverter<TSource,TTarget> : IValueConverter
    {
        public abstract TSource ConvertBack(TTarget target, CultureInfo culture);

        public abstract bool CanConvertBack(TTarget target, CultureInfo culture);

        object IValueConverter.Convert(object source, CultureInfo culture)
        {
            throw new NotSupportedException(string.Format(
                "The '{0}' convert does not support conversion from source.",
                GetType().Name));
        }

        bool IValueConverter.CanConvert(object source, CultureInfo culture)
        {
            return false;
        }

        object IValueConverter.ConvertBack(object target, CultureInfo culture)
        {
            return ConvertBack((TTarget) target, culture);
        }

        bool IValueConverter.CanConvertBack(object target, CultureInfo culture)
        {
            return CanConvertBack((TTarget) target, culture);
        }
    }
}
