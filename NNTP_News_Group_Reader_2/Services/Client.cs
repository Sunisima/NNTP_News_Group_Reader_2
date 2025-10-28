using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;

namespace NNTP_News_Group_Reader_2.Services
{
    public class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _streamReader;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"> The username of the user wanting to log in </param>
        /// <param name="password"> the password for the user </param>
        /// <returns></returns>
        public string Login(string userName, string password)
        {
            // Login with user name and await server response
            string userAuth = CommandsAndResponses($"AUTHINFO USER {userName}");

            //Send password to server for validation
            string passwordAuth = CommandsAndResponses($"AUTHINFO PASS {password}");

            return passwordAuth;            
        }
    

        /// <summary>
        /// Connecting to the NNTP-server
        /// </summary>
        /// <param name="host"> The URL of the server </param>
        /// <param name="portNo"> The port number to use to get connection </param>
        public void ConnectToServer (string host, int portNo)
        {
            _client = new TcpClient(host, portNo);
            _stream = _client.GetStream();
            _streamReader = new StreamReader(_stream, Encoding.ASCII);
        }


        /// <summary>
        /// Used for sending commands from client to server and recieving responses
        /// </summary>
        /// <param name="command"> The specific commands send to the server </param>
        public string CommandsAndResponses (string command)
        {
            byte[] queries = Encoding.ASCII.GetBytes(command + "\r\n");
            _stream.Write(queries, 0, queries.Length);
            return _streamReader.ReadLine();
        }

        public void Close()
        {
            _streamReader.Close();
            _stream.Close();
            _client.Close();
        }
    }
}
