using System;
using System.Windows.Data;
using System.Globalization;

namespace GUIClient
{
    /// <summary>
    /// Converts sacles between GUI and data.
    /// For example, in the data, every square in the maze in 1 unit but in the
    /// GUI every square is more then 1 pixels, So this converts between the two.
    /// </summary>
    public class MazeSizeConverter : IValueConverter
    {
        /// <summary>
        /// Convert from data scales to GUI scales.
        /// </summary>
        /// <param name="value"> An int. </param>
        /// <param name="targetType"> Meaningless. </param>
        /// <param name="parameter"> Meaningless. </param>
        /// <param name="culture"> Meaningless. </param>
        /// <returns> An int(as Object) </returns>
        public object Convert(object value, Type targetType, object parameter,
                            CultureInfo culture)
        {
            //Convert to be 30 times the rows and cols.
            return (int)30 * (int)value;
        }

        /// <summary>
        /// Convert to data scales from GUI scales.
        /// </summary>
        /// <param name="value"> An int. </param>
        /// <param name="targetType"> Meaningless. </param>
        /// <param name="parameter"> Meaningless. </param>
        /// <param name="culture"> Meaningless. </param>
        /// <returns> An int(as Object) </returns>
        public object ConvertBack(object value, Type targetType, object parameter,
                                    CultureInfo culture)
        {
            return (int)((int)value / 30);
        }
    }
}
