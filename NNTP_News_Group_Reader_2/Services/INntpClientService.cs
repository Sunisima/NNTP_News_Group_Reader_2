using NNTP_News_Group_Reader_2.Model;

namespace NNTP_News_Group_Reader_2.Services
{
    /// <summary>
    /// Defines the basic operations required for communicating with an NNTP-server.
    /// Loose coupling from our Service-layer
    /// </summary>
    public interface INntpClientService
    {
        Task ConnectToServerAsync(string host, int portNo);
        Task<string> LoginAsync(string username, string password);
        Task<string> CommandsAndResponsesAsync(string command);
        void CloseConnection();
        Task<List<string>> ListCommandToServerAsync();
        public List<NewsGroups> ParseNewsGroupsToObjects(List<string> rawDataLines);
        Task<List<ArticleHeadlines>> FetchArticleHeadlines(string selectedGroup);
        Task<string> GetFullArticleBody(int articleId);
    }
}
