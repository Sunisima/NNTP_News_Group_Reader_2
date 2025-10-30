namespace NNTP_News_Group_Reader_2.Model
{
    public class NewsGroups
    {
        public string NewsGroupName { get; set; }
        //The article with the lowestID is the first article posted in that group
        public int FirstArticleId { get; set; }
        //The article with the highestID is the last article posted in that group
        public int LastArticleId { get; set; }
        public char flagStatus { get; set; }
    }
}
