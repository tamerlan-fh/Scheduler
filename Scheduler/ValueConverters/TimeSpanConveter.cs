using System;
using System.Globalization;
using System.Windows.Data;

namespace Scheduler.ValueConverters
{
    class TimeSpanConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((TimeSpan)value).ToString(@"hh\:mm\:ss");
            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time;
            if (TimeSpan.TryParse(value.ToString(), out time))
                return time;
            return null;
        }
    }
}
