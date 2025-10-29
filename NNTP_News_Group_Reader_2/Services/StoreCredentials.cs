using Newtonsoft.Json;
using System.IO;

namespace NNTP_News_Group_Reader_2.Services
{
    /// <summary>
    /// A class that saves and retrieves user login credentials for a specific NNTP server and port.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class StoreCredentials
    {
        private readonly string _jsonFile = "UserLoginCredentials.json";


        [JsonProperty("host_name", Order = 1)] // URL of the server
        public string Host {  get; set; }

        [JsonProperty("port_no", Order = 2)] // Port number used to contact server
        public int PortNo { get; set; }

        [JsonProperty("username", Order = 3)] // The username of the user
        public string UserName { get; set; }

        [JsonProperty("password", Order = 4)] // User's password to login to server
        public string Password { get; set; }

        /// <summary>
        /// Saving user login credentials to a json-file
        /// </summary>
        /// <param name="loginCredentials"> The user credentials to be stored to the file </param>
        public void SaveLoginCredentials(StoreCredentials loginCredentials)
        {
            try
            {
                string saveToJson = JsonConvert.SerializeObject(loginCredentials, Formatting.Indented);
                File.WriteAllText(_jsonFile, saveToJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not save your credentials: " +  ex.Message);
            }
        }

        /// <summary>
        /// Loading saved user login credentials from the json-file is the user have logged ind before.
        /// </summary>
        public StoreCredentials LoadLoginCredentials()
        {
            if (!File.Exists(_jsonFile))
            {
                return null;
            }
                string readFromJson = File.ReadAllText(_jsonFile);
                return JsonConvert.DeserializeObject<StoreCredentials>(readFromJson); // Reads from the Json-file and deserializes the data
        }
    }
}
