using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NNTP_News_Group_Reader_2.Model;
using NNTP_News_Group_Reader_2.Services;
using System.Collections.ObjectModel;

namespace NNTP_News_Group_Reader_2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INntpClientService _nntpClient;
        private readonly BindingToNntpClientService _bindingNntpService;
        private string _connectionStatus;
        private bool _isConnected;

        public User User { get; set; }
        // Binds UI-element directly to methods in the ViewModel-layer
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand LoadNewsGroupsCommand { get; set; }

        // Gets or sets the current connection status message displayed in the UI.
        public string ConnectionStatus
        {

            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                RaisePropertyChanged(); // Tells UI to update with new values
            }
        }


        // Checks whether the user is connected to the server or not
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                RaisePropertyChanged();
            }
        }


        // Holds the collection of news groups displayed in the UI.
        public ObservableCollection<NewsGroups> NewsGroups { get; set; } = new();


        /// <summary>
        /// Initializes the MainWindowViewModel by creating instances of the NNTP client and user model.
        /// </summary>
        public MainWindowViewModel()
        {
            _nntpClient = new NntpClient();
            _bindingNntpService = new BindingToNntpClientService(_nntpClient);
            
            User = new User();

            ConnectCommand = new RelayCommand(ConnectToServer);
            //Binds UI to method ind ViewModel
            LoadNewsGroupsCommand = new RelayCommand(async () => await LoadNewsGroups());
        }        


        /// <summary>
        /// Connects to the NNTP server using the information from the User object.
        /// Updates the ConnectionStatus and saves the credentials if login succeeds.
        /// </summary>
        public async void ConnectToServer()
        {
            try
            {
                // Update UI status
                ConnectionStatus = "Connecting...";
                // Connect to the server
                await _nntpClient.ConnectToServerAsync(User.Host, User.PortNo);
                // Attempt login with username and password
                string response = await _nntpClient.LoginAsync(User.UserName, User.Password);

                // Response from server
                if (response.StartsWith("281"))
                {
                    ConnectionStatus = "Connection Successful";
                    IsConnected = true;

                    // Save credentials
                    var storeLogin = new StoreCredentials
                    {
                        Host = User.Host,
                        PortNo = User.PortNo,
                        UserName = User.UserName,
                        Password = User.Password
                    };

                    storeLogin.SaveLoginCredentials(storeLogin);
                }
                else
                {
                    ConnectionStatus = "Connection failed - Try again";
                    IsConnected = false;
                }
            }
            catch (Exception ex)
            {
                // Handles connection or login errors
                ConnectionStatus = "Connection error: " + ex.Message;
            }
        }


        /// <summary>
        /// Loads all available news groups from the NNTP server and updates the observable collection in the UI.
        /// </summary>
        public async Task LoadNewsGroups()
        {
            try
            {
                ConnectionStatus = "Fetching news groups...";

                var groups = await _bindingNntpService.GetAllNewsGroupsAsync();

                NewsGroups.Clear();

                foreach (var group in groups)
                {
                    NewsGroups.Add(group);
                }

                ConnectionStatus = "Loaded successfully";
            }
            catch (Exception ex)
            {
                ConnectionStatus = "Error loading groups: " + ex.Message;
            }
        }


        /// <summary>
        /// Gets all article headlines for the selected news group by calling the NNTP client service.
        /// </summary>

        public async Task<List<ArticleHeadlines>> GetArticlesForSelectedGroup(string groupName)
        {
            return await _nntpClient.FetchArticleHeadlines(groupName);
        }


        /// <summary>
        /// Gets the full text of a specific article by calling the NNTP client service.
        /// Used by the UI when a headline is selected.
        /// </summary>
        public async Task<string> GetArticleBody(int articleId)
        {
            return await _nntpClient.GetFullArticleBody(articleId);
        }

    }
}
