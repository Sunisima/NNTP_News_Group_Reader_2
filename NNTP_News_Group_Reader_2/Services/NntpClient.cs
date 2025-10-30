using NNTP_News_Group_Reader_2.Model;
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


        /// <summary>
        /// Sends the command LIST to the server to get all news groups and return them in a string.
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> ListCommandToServerAsync()
        {
            var result = new List<string>();

            byte[] listToByte = Encoding.ASCII.GetBytes("LIST \r\n");
            await _stream.WriteAsync(listToByte, 0, listToByte.Length);

            string? response = await _streamReader.ReadLineAsync();
            if (response == null || !response.StartsWith("215"))
                return result;

            string? readAllLines; // Until it reaches "."
            while ((readAllLines = await _streamReader.ReadLineAsync()) !=null)
            {
                if (readAllLines == ".")
                    break;
                result.Add(readAllLines);
            }
            return result;  
        }


        /// <summary>
        /// Parsing raw data from the server to NewsGroups objects
        /// </summary>
        /// <param name="rawDataLines"></param>
        /// <returns></returns>
        public List<NewsGroups> ParseNewsGroupsToObjects(List<string> rawDataLines)
        {
            var newsGroupsResult = new List<NewsGroups>();

            foreach (var line in rawDataLines)
            {
                var parting = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parting.Length >= 4 && !parting[0].StartsWith("215"))
                {
                    newsGroupsResult.Add(new NewsGroups
                    {
                        NewsGroupName = parting[0],
                        LastArticleId = int.TryParse(parting[1], out var high) ? high : 0,
                        FirstArticleId = int.TryParse(parting[2], out var low) ? low : 0,
                        flagStatus = parting[3][0]
                    });
                }
            }
            return newsGroupsResult;
        }


        /// <summary>
        /// Fetches and parses all article headlines for a specific news group from the NNTP server.
        /// Sends NNTP commands:
        /// 1) GROUP <selectedGroup> – to select the group and get its article range
        /// 2) XOVER <low>-<high> – to retrieve overview data for all articles in that range
        /// The response is parsed line by line, split by tab characters, and 
        /// converted into a list of ArticleHeadlines objects.
        /// </summary>
        /// <param name="selectedGroup">The name of the news group selected by the user.</param>
        /// <returns>A list of ArticleHeadlines containing the article ID and title.</returns>
        public async Task<List<ArticleHeadlines>> FetchArticleHeadlines(string selectedGroup)
        {
            var headlines = new List<ArticleHeadlines>();

            //Get selected group
            string selectedGroupCommand = $"GROUP {selectedGroup}\r\n";
            byte[] groupQuery = Encoding.ASCII.GetBytes(selectedGroupCommand);
            await _stream.WriteAsync(groupQuery, 0, groupQuery.Length);
            string? groupResponse = await _streamReader.ReadLineAsync();

            // 2) Parse lowest og highest article number from  the result
            var groupParts = groupResponse?.Split(' ');
            if (groupParts == null || groupParts.Length < 5)
                throw new Exception("Error, No articles found.");
            //Takes the first and last articleNo from the result. It is used to show all articles in the group
            string lowestArticleNo = groupParts[2];
            string highestArticleNo = groupParts[3];

            // 3) Gets all articles with the range of above chosen numbers
            string xoverCommand = $"XOVER {lowestArticleNo}-{highestArticleNo}\r\n";
            await _stream.WriteAsync(Encoding.ASCII.GetBytes(xoverCommand));

            // 4) Read all headlines until it reaches "."
            while (true)
            {
                string? line = await _streamReader.ReadLineAsync();
                if (line == null || line == ".")
                    break;

                // 5) Split lines on each tab = one article
                var parts = line.Split('\t');
                if (parts.Length > 1)
                {
                    var headline = new ArticleHeadlines
                    {
                        ArticleId = int.Parse(parts[0]),
                        HeadlineTitle = MimeDecoder.Decode(parts[1])
                    };
                    headlines.Add(headline);
                }
            }

            return headlines;
        }
    }
}
