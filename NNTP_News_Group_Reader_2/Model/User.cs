namespace NNTP_News_Group_Reader_2.Model
{
    /// <summary>
    /// A class that stores all credentials related to the NNTP server and user.
    /// </summary>
    public class User
    {
        public string Host { get; set; } = "news.sunsite.dk";
        public int PortNo { get; set; } = 119;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
