using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NNTP_News_Group_Reader_2.Converter
{
    /// <summary>
    /// A converter used in Xaml to show or hide UI elements.
    /// Hides elements in the UI when the user is connected to the server.
    /// </summary>
    public class VisibilityOffConv : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value from the ViewModel into a Visibility value for the UI.
        /// If the user is connected (true), the element will be hidden (collapsed).
        /// If the user is not connected (false), the element will be visible.
        /// </summary>
        /// <returns>Returns Visibility.Collapsed if connected (true); otherwise Visibility.Visible.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
                return isConnected ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
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
