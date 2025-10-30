namespace NNTP_News_Group_Reader_2.Model
{
    public class NewsGroups
    {
        public string NewsGroupName { get; set; }
        public int FirstArticleId { get; set; }
        public int LastArticleId { get; set; }
        public char flagStatus { get; set; }
    }
}
