using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TaxonomiaWeb.XAMLConverters
{
    public class MyConverterRow : IValueConverter
    {
        bool flag = false;
        SolidColorBrush brush1 = new SolidColorBrush(Color.FromArgb(255, 100, 200, 255));
        SolidColorBrush brush2 = new SolidColorBrush(Color.FromArgb(255, 200, 100, 155));
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
        {

            flag = !flag;
            return flag ? brush1 : brush2;

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
