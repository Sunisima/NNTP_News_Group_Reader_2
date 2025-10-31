using NNTP_News_Group_Reader_2;

namespace NNTP_Tests
{
    public class Us4_ShowFullArticleBodyTest
    {
        [Fact]
        public void ShowFullArticle_ReturnOK()
        {
            // Arrange
            var article = new Us4_FullArticle();

            // Act
            string result = article.GetFullArticle("12345");

            // Assert
            Assert.Equal("Sådan virker NNTP", result);
        }
    }
}
