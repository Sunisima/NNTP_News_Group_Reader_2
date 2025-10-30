using NNTP_News_Group_Reader_2.Model;

namespace NNTP_News_Group_Reader_2.Services
{
    public class BindingToNntpClientService
    {
        private readonly INntpClientService _nntpClient;

        public BindingToNntpClientService(INntpClientService nntpClient) 
        {
            _nntpClient = nntpClient; // Dependency injection here
        }


        /// <summary>
        /// Gets and returns a list of news groups through the NntpClient-class
        /// </summary>
        /// <returns></returns>
        public async Task<List<NewsGroups>> GetAllNewsGroupsAsync()
        {
            var rawData = await _nntpClient.ListCommandToServerAsync();
            var parseData = ((NntpClient)_nntpClient).ParseNewsGroupsToObjects(rawData);
            return parseData;
        }

    }
}
