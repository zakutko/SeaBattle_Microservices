using Identity.BLL.Interfaces;
using Moq;
using SeaBattle.Contracts.Dtos;

namespace Identity.BLL.Tests
{
    public class IdentityServiceTests
    {
        private const string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlRlc3QiLCJuYW1laWQiOiI0NmIyZTAzMy02MDYzLTRmZWQtOTI2MS1lMmIwYjJiNzQwNTIiLCJlbWFpbCI6InRlc3RAZ21haWwuY29tIiwibmJmIjoxNjY5OTg2ODk3LCJleHAiOjE2NzA1OTE2OTcsImlhdCI6MTY2OTk4Njg5N30.OaukKIoZeAOISA2llJZaYz93VltW9cvR6S5pr7RLHQphPxFK6_L05Z4gWWtKKej1TokN9pm46K1Z4nSiHIoLAQ";

        private readonly Mock<IIdentityService> _mock = new Mock<IIdentityService>();

        [Fact]
        public void GetCurrUser_should_return_usermame_and_token()
        {
            //Arrange
            var getCurruserRequest = new GetCurrUserRequest
            {
                Token = token
            };

            var fakeResponse = new UserResponse
            {
                Username = "Test",
                Token = token
            };

            _mock.Setup(x => x.GetCurrUser(getCurruserRequest)).Returns(fakeResponse);

            //Act
            var result = _mock.Object.GetCurrUser(getCurruserRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(fakeResponse.Token, result.Token);
            Assert.Equal(fakeResponse.Username, result.Username);
        }

        [Fact]
        public void Login_if_user_does_not_exist_throw_exeption()
        {
            //Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var fakeExeption = "User does not exist!";

            _mock.Setup(x => x.Login(loginRequest)).Throws(new Exception(fakeExeption));
            //Act
            //Assets
            try
            {
                var result = _mock.Object.Login(loginRequest);
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Equal(fakeExeption, ex.Message);
            }
        }

        [Fact]
        public void Login_if_user_exist_but_password_wrong_throw_Exeption()
        {
            //Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var fakeExeption = "Invalid password!";

            _mock.Setup(x => x.Login(loginRequest)).Throws(new Exception(fakeExeption));

            //Act
            //Assert
            try
            {
                var result = _mock.Object.Login(loginRequest);
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Equal(fakeExeption, ex.Message);
            }
        }

        [Fact]
        public void Login_if_successful_return_userResponse()
        {
            //Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "password"
            };

            var fakeUserResponse = new UserResponse
            {
                Username = "Test",
                Token = token
            };

            _mock.Setup(x => x.Login(loginRequest).Result).Returns(fakeUserResponse);

            //Act
            var result = _mock.Object.Login(loginRequest).Result;

            //Assets
            Assert.NotNull(result);
            Assert.Equal(fakeUserResponse, result);
        }

        [Theory]
        [InlineData("Email taken!")]
        [InlineData("Username taken!")]
        [InlineData("Problem registering user!")]
        public void Register_if_unsuccessful_throw_any_exeption(string exeptionMessage)
        {
            //Arrange
            var registerRequest = new RegisterRequest
            {
                UserName = "Test",
                DisplayName = "Test",
                Email = "test@gmail.com",
                Password = "password"
            };

            var fakeExeptionMessage = exeptionMessage;

            _mock.Setup(x => x.Register(registerRequest)).Throws(new Exception(fakeExeptionMessage));

            //Act
            //Assert
            try
            {
                var result = _mock.Object.Register(registerRequest).Result;
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Equal(fakeExeptionMessage, ex.Message);
            }
        }

        [Fact]
        public void Register_if_successfull_return_userResponse()
        {
            //Arrange
            var registerRequest = new RegisterRequest
            {
                UserName = "Test",
                DisplayName = "Test",
                Email = "test@gmail.com",
                Password = "password"
            };

            var fakeUserResponse = new UserResponse
            {
                Username = "Test",
                Token = token
            };

            _mock.Setup(x => x.Register(registerRequest).Result).Returns(fakeUserResponse);

            //Act
            var result = _mock.Object.Register(registerRequest).Result;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(fakeUserResponse, result);
        }
    }
}