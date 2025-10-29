using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NNTP_News_Group_Reader_2.Model;
using NNTP_News_Group_Reader_2.Services;

namespace NNTP_News_Group_Reader_2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INntpClientService _nntpClient;
        private string _connectionStatus;
      

        public User User { get; set; }
        public RelayCommand ConnectCommand { get; set; }


        /// <summary>
        /// Gets or sets the current connection status message displayed in the UI.
        /// Notifies the View when the value changes.
        /// </summary>
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                RaisePropertyChanged(); // Tells UI to update with new values
            }
        }


        /// <summary>
        /// Initializes the MainWindowViewModel by creating instances of the NNTP client and user model.
        /// </summary>
        public MainWindowViewModel()
        {
            _nntpClient = new NntpClient();
            User = new User();
            ConnectCommand = new RelayCommand(ConnectToServer);
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
                    ConnectionStatus = "Connection Successfull";

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
                }
            }
            catch (Exception ex)
            {
                // Handles connection or login errors
                ConnectionStatus = "Connection error: " + ex.Message;
            }
        }
    }
}
