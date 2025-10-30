using System.Net.Sockets;
using System.Text;

namespace NNTP_Spikes
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Spike 1: Making sure that a connection to the server with username and password is possible.
            const string host = "news.sunsite.dk";// an open news-server (Host B)
            const int port = 119;

            TcpClient client = null;
            NetworkStream ns = null;
            StreamReader reader = null; // Read from host B

            // (1) Create an empty client-object
            client = new TcpClient();

            // (2) Short timeouts to cast exceptions if no connetion is established within 10 sec.
            client.ReceiveTimeout = 10000;
            client.SendTimeout = 10000;

            // (3) Connect to the server - Calling host B
            client.Connect(host, port);

            // 4) Get network stream - only possible after connection is estblished.
            ns = client.GetStream();

            // 5) Create a reader (responds with ASCII-text)
            reader = new StreamReader(ns, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, bufferSize: 1024);

            // 6) Read the lines and write it to the console
            string connectedToServer = reader.ReadLine();
            Console.WriteLine(connectedToServer);

            // 7) Login with username
            string usernameAuth = "AUTHINFO USER loulee01@easv365.dk\r\n";
            byte[] usernameQuery = Encoding.ASCII.GetBytes(usernameAuth); // Text to binary data
            ns.Write(usernameQuery, 0, usernameQuery.Length); // Send query to open network stream to NNTP-server
            string usernameResponse = reader.ReadLine();
            Console.WriteLine(usernameResponse);

            // 8) Login with password
            string passwordAuth = "AUTHINFO PASS 47b31a\r\n";
            byte[] passwordQuery = Encoding.ASCII.GetBytes(passwordAuth);
            ns.Write(passwordQuery, 0, passwordQuery.Length);
            string passResponse = reader.ReadLine();
            Console.WriteLine(passResponse);


            Console.WriteLine();


            // Spike 2: Check if LIST gets and shows all available news groups. The command returns all group names and their metadata

            // 1) Send LIST command to server
            string listCommand = "LIST \r\n";
            byte[] listToByte = Encoding.ASCII.GetBytes(listCommand);
            ns.Write(listToByte, 0, listToByte.Length);
            ns.Flush();

            Console.WriteLine(reader.ReadLine()); // 215 ...

            // Read ALL lines until "."
            string? listAllLines;
            while ((listAllLines = reader.ReadLine()) != null)
            {
                if (listAllLines == ".") break;
                Console.WriteLine(listAllLines);
            }           

            Console.WriteLine();



            // Spike 3: Check if XOVER gets headlines for a specific news group

            // 1) Chose a group to test out
            string selectedGroup = "dk.fritid.bil";
            string groupCommand = $"GROUP {selectedGroup}\r\n";
            byte[] groupQuery = Encoding.ASCII.GetBytes(groupCommand);
            ns.Write(groupQuery, 0, groupQuery.Length);
            ns.Flush();

            string? groupResponse = reader.ReadLine();
            Console.WriteLine("Server response: " + groupResponse);

            // 2) Server gives overview of article headlines
            string xoverCommand = "XOVER 773650-773660\r\n";
            byte[] xoverQuery = Encoding.ASCII.GetBytes(xoverCommand);
            ns.Write(xoverQuery, 0, xoverQuery.Length);
            ns.Flush();

            Console.WriteLine("Sent XOVER command for dk.fritid.bil\n");

            // Read headlines from articles
            string? xoverHeadLLine;
            while ((xoverHeadLLine = reader.ReadLine()) != null)
            {
                if (xoverHeadLLine == ".") break;
                Console.WriteLine(xoverHeadLLine);
            }

        }
    }
}
