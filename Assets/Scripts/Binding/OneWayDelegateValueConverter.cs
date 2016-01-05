using JetBrains.Annotations;
using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public class OneWayDelegateValueConverter<TSource, TTarget> : OneWayValueConverter<TSource, TTarget>
    {
        private readonly Func<TSource, CultureInfo, bool> _canConvert;
        private readonly Func<TSource, CultureInfo, TTarget> _convert;


        public OneWayDelegateValueConverter([NotNull] Func<TSource, CultureInfo, TTarget> convert)
            : this(convert, (source, culture) => true)
        {
        }

        public OneWayDelegateValueConverter(
            [NotNull] Func<TSource, CultureInfo, TTarget> convert,
            [NotNull] Func<TSource, CultureInfo, bool> canConvert)
        {
            if (convert == null)
            {
                throw new ArgumentNullException("convert");
            }
            if (canConvert == null)
            {
                throw new ArgumentNullException("canConvert");
            }

            _convert = convert;
            _canConvert = canConvert;
        }

        public override TTarget Convert(TSource source, CultureInfo culture)
        {
            return _convert(source, culture);
        }

        public override bool CanConvert(TSource source, CultureInfo culture)
        {
            return _canConvert(source, culture);
        }
    }
}
