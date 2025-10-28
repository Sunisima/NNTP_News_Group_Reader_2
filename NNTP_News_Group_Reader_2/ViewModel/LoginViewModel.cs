using GalaSoft.MvvmLight;
using NNTP_News_Group_Reader_2.Services;

namespace NNTP_News_Group_Reader_2.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INntpClientService _nntpClient;

        public LoginViewModel()
        {
            _nntpClient = new NntpClient();
        }
    }
}
