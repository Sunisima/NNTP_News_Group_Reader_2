using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNTP_News_Group_Reader_2.Converter
{
    /// <summary>
    /// A converter used in Xaml to show or hide UI elements.
    /// Shows elements in the UI when the user is connected to the server.
    /// </summary>
    public class VisibilityOnConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value from the ViewModel to a Visibility value for the UI.
        /// If the user is connected (true), the element becomes visible.
        /// If not connected (false), the element is hidden (collapsed).
        /// </summary>
        /// <returns>  Returns Visibility.Visible if the user is connected (true);
        /// otherwise returns Visibility.Collapsed to hide the element. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
                return isConnected ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Not used in my project. Only used if a user changes anythin in the UI.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
