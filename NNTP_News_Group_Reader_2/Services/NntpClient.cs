using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NNTP_News_Group_Reader_2.Services
{
    /// <summary>
    /// Handles the communication between the client and the NNTP-server.
    /// Implements the INntpClientService interface and contains methods for 
    /// connecting, logging in, sending commands, and closing the connection.
    /// </summary>
    public class NntpClient : INntpClientService
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _streamReader;


        /// <summary>
        /// Connects to the NNTP-server and prepares the stream and reader for communication.
        /// </summary>
        /// <param name="host">The address of the NNTP-server.</param>
        /// <param name="portNo">The port number used for connection.</param>
        /// <returns></returns>
        public async Task ConnectToServerAsync(string host, int portNo)
        {
            // Creates a TCP connection to the NNTP-server
            _client = new TcpClient();
            await _client.ConnectAsync(host, portNo);  // Connects asynchronously to the server and port

            // Gets the network stream to send and receive data
            _stream = _client.GetStream();
            _streamReader = new StreamReader(_stream, Encoding.ASCII); // Creates a StreamReader to read server responses

            // Reads the first line sent from the server 
            _ = await _streamReader.ReadLineAsync();
        }


        /// <summary>
        /// Logs in to the NNTP-server with username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The response from the server after login.</returns>
        public async Task<string> LoginAsync(string userName, string password)
        {
            //Login with user name and await server response
            string userAuth = await CommandsAndResponsesAsync($"AUTHINFO USER {userName}");

            //Send password to server for validation
            string passwordAuth = await CommandsAndResponsesAsync($"AUTHINFO PASS {password}");

            return passwordAuth;
        }


        ///// <summary>
        ///// Used for sending commands from client to server and receiving responses
        ///// </summary>
        ///// <param name="command"> The specific commands sent to the server </param>
        public async Task<string> CommandsAndResponsesAsync(string command)
        {
            // Converts the command to bytes and sends it to the server
            byte[] queries = Encoding.ASCII.GetBytes(command + "\r\n");
            await _stream.WriteAsync(queries, 0, queries.Length);

            // Reads and returns the response from the server
            return await _streamReader.ReadLineAsync();
        }

        /// <summary>
        /// Closes all open connections in reverse order to free up resources.
        /// </summary>
        public void CloseConnection()
        {
            _streamReader.Close();
            _stream.Close();
            _client.Close();
        }
    }
}
