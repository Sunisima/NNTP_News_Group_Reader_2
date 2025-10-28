using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;

namespace NNTP_News_Group_Reader_2
{
    public class Client
    {
        public string Login(string host, int portNo, string userName, string password) 
        {
            //Connects to NNTP-server, reads and sends files through networdStream anf creates a reader to read the text from the server.
            using var client = new TcpClient(host, portNo);
            using var stream = client.GetStream();
            using var streamReader = new StreamReader(stream, Encoding.ASCII);

            try
            {
                // Reads server response
                string serverConnection = streamReader.ReadLine();

                // Login with user name and await server response
                string userAuth = $"AUTHINFO USER {userName}\r\n";
                byte[] usernameQuery = Encoding.ASCII.GetBytes(userAuth);
                stream.Write(usernameQuery, 0, usernameQuery.Length); 
                string userResponse = streamReader.ReadLine();

                //Send password to server for validation
                string passwordAuth = $"AUTHINFO PASS {password}\r\n";
                byte[] passwordQuery = Encoding.ASCII.GetBytes(passwordAuth);
                stream.Write(passwordQuery, 0, passwordQuery.Length);

                string passResponse = streamReader.ReadLine();

                return passResponse;
            }
            catch (Exception ex)
            {
                // Return an error message
                Console.WriteLine("Fejl: " + ex.Message);
                return "Login failed: " + ex.Message;
            }
        }
    }
}
