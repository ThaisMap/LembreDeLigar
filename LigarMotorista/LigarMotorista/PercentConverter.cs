using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LigarMotorista
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            var valor = (int)(int.Parse(value.ToString()) * double.Parse(parameter.ToString()) / 100);
            return valor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valor = (int)(int.Parse(value.ToString()) / double.Parse(parameter.ToString()) / 100);
            return valor;
        }
    }
}