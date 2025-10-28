using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NNTP_News_Group_Reader_2
{
    public class Us1_Login
    {
        // Red phase - it is supposed to fail and does.
        //public string Login(string host, int portNo, string userName, string password)
        //{
        //    return string.Empty;
        //}


        // Green phase - Minimal implementation that returns expected value to make the unit test pass.
        public string Login(string host, int portNo, string userName, string password)
        {
            return "281 Ok";
        }
    }
}
