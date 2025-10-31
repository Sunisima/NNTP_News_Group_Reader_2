using NNTP_News_Group_Reader_2;

namespace NNTP_Tests
{
    public class Us1_LoginTest
    {
        [Fact]
        public void LoginWithCredentials_ReturnsOk()
        {
            // Arrange
            var client = new Us1_Login();

            // Act
            string actual = client.Login("news.sunsite.dk", 119, "loulee01@easv365.dk", "47b31a");

            // Assert
            Assert.Equal("281 Ok", actual);
        }
    }
}
