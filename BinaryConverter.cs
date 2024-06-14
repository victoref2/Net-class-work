using System;
using System.Globalization;
using System.Windows.Data;


namespace Number_system_conveter
{
    public class BinaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int number))
            {
                return System.Convert.ToString(number, 2).PadLeft(8, '0');
            }
            return "00000000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string binaryString)
            {
                try
                {
                    return System.Convert.ToInt32(binaryString, 2);
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }
    }

}