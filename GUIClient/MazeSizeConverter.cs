using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace GUIClient
{
    public class MazeSizeConverter : IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,
                            CultureInfo culture)
        {
            //Convert to be 30 times the rows and cols.
            //TODO- handle type safety later.
            return (int)30 * (int)value;
        }

        public object ConvertBack(object value,Type targetType,object parameter,
                                    CultureInfo culture)
        {
            //TODO- handle exceptions based on type later.
            return (int)((int)value / 30);
        }
    }
}
