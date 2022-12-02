using Identity.BLL.Interfaces;
using Identity.BLL.Services;
using Identity.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Identity.BLL.Tests
{
    public class TokenServiceTests
    {
        [Fact]
        public void CreateToken_MustBeNotNullOrEmpty_ReturnTrue()
        {
            //Arrange
            var appUser = new AppUser
            {
                DisplayName = "Test",
                Email = "test@gmail.com",
                UserName = "Test"
            };

            var tokenService = new TokenService();

            //Act
            var result = tokenService.CreateToken(appUser);

            //Assert
            Assert.True(!result.IsNullOrEmpty());
        }
    }
}