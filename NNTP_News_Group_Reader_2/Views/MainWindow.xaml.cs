using NNTP_News_Group_Reader_2.Model;
using NNTP_News_Group_Reader_2.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace NNTP_News_Group_Reader_2.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Finds selected group in the NewsGroupList in Xaml and show headlines in ArticleList in the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetArticlesFromSelectedGroup (object sender, SelectionChangedEventArgs e)
        {
            if(NewsGroupList.SelectedItem is NewsGroups selectedGroup)
            {
                try
                {
                    var groupHeadlines = await _viewModel.GetArticlesForSelectedGroup(selectedGroup.NewsGroupName);
                  
                    ArticleList.ItemsSource = groupHeadlines;
                    ArticleList.DisplayMemberPath = "HeadlineTitle";
                }
                catch (Exception ex)
                {
                    {
                        MessageBox.Show($"Kunne ikke hente artikler for '{selectedGroup.NewsGroupName}'.\nFejl: {ex.Message}",
                        "Fejl ved hentning", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}