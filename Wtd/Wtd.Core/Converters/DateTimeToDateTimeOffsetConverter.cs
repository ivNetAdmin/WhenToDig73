using System;
using System.Globalization;
using Xamarin.Forms;

namespace Wtd.Core.Converters
{
    public class DateTimeOffsetToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //try
            //{
            //    DateTime date = (DateTime)value;
            //    return new DateTimeOffset(date);
            //}
            //catch
            //{
            //    return DateTimeOffset.MinValue;
            //}

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
            //try
            //{
            //    DateTimeOffset dto = (DateTimeOffset)value;
            //    return dto.DateTime;
            //}
            //catch
            //{
            //    return DateTime.MinValue;
            //}

        }
    }
}
