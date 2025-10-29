using NNTP_News_Group_Reader_2.ViewModel;
using System.Windows;

namespace NNTP_News_Group_Reader_2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}