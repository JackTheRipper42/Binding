using System;
using System.Globalization;

namespace Assets.Scripts.Binding
{
    public interface IValueConverter
    {
        object Convert(object source, CultureInfo culture);

        bool CanConvert(object source, CultureInfo culture);

        object ConvertBack(object target, CultureInfo culture);

        bool CanConvertBack(object target, CultureInfo culture);
    }
}
