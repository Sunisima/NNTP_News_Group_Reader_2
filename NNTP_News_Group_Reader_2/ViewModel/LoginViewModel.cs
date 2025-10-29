using GalaSoft.MvvmLight;
using NNTP_News_Group_Reader_2.Model;
using NNTP_News_Group_Reader_2.Services;

namespace NNTP_News_Group_Reader_2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INntpClientService _nntpClient;
        private readonly User _user;

        public MainWindowViewModel()
        {
            _nntpClient = new NntpClient();
            _user = new User();
        }
    }
}
