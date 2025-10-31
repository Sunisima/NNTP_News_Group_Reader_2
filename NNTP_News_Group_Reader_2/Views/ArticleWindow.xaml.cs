using System.Windows;

namespace NNTP_News_Group_Reader_2.Views
{
    public partial class ArticleWindow : Window
    {
        public ArticleWindow(string articleBody)
        {
            InitializeComponent();
            ArticleContent.Text = articleBody;
        }
    }
}
