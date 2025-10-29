namespace NNTP_News_Group_Reader_2.Services
{
    /// <summary>
    /// Defines the basic operations required for communicating with an NNTP-server.
    /// Loose coupling from our Services.
    /// </summary>
    public interface INntpClientService
    {
        Task ConnectToServerAsync(string host, int portNo);
        Task<string> LoginAsync(string username, string password);
        Task<string> CommandsAndResponsesAsync(string command);
        void CloseConnection();
    }
}
