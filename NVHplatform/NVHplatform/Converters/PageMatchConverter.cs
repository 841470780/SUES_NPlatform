using System;
using System.Globalization;
using System.Windows.Data;

namespace NVHplatform.Converters
{
    public class PageMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string currentPage = value.ToString();
            string page = parameter.ToString();

            return string.Equals(currentPage, page, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }


}
