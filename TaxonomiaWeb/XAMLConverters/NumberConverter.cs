using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace TaxonomiaWeb.XAMLConverters
{

        //[ValueConversion(typeof(object), typeof(string))]
        public class NumberConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string str = value as string;
                if (string.IsNullOrEmpty(str)) return str;
                str = str.Insert(1, "'");
                str = str + "\"";
                return str;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return 0;
            }

            object Format(object value, object param)
            {
                if (value == null)
                    return value;

                string newValue = value.ToString();
                double rd = 0;
                string returnValue = rd.ToString();

                try
                {
                    if (double.TryParse(newValue, out rd))
                    {
                        returnValue = rd.ToString();
                    }
                }
                catch 
                {
 
                }

                return returnValue;

            }

        }

}
