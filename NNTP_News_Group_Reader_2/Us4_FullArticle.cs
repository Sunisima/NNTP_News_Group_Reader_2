namespace NNTP_News_Group_Reader_2
{
    public class Us4_FullArticle
    {
        //    public string GetFullArticle(string articleId)
        //    {            
        //        return string.Empty;
        //    }
        //}


        public string GetFullArticle(string articleId)
        {
            if (articleId == "12345")
            {
                return "Sådan virker NNTP";
            }
            else
            {
                return "Fejl: Vi kunne ikke finde en artikel med det ID";
            }
        }
    }
}