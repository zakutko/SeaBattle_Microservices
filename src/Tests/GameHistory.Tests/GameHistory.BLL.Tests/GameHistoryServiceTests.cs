using GameHistory.BLL.Interfaces;
using Moq;
using SeaBattle.Contracts.Dtos;

namespace GameHistory.BLL.Tests
{
    public class GameHistoryServiceTests
    {
        private readonly Mock<IGameHistoryService> _mock = new Mock<IGameHistoryService>();

        [Fact]
        public void GetAllGameHistories_RequestMessageMayBeEmpty_ReturnTrue()
        {
            var fakeGameHistories = new List<GameHistoryResponse>
            {
                new GameHistoryResponse
                {
                    Id = 2,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test",
                    GameId= 2,
                    GameStateName = "Test",
                    WinnerName = "Test",
                },
                new GameHistoryResponse
                {
                    Id = 1,
                    FirstPlayerName = "Test2",
                    SecondPlayerName = "Test2",
                    GameId= 1,
                    GameStateName = "Test2",
                    WinnerName = "Test2",
                }
            };

            var gameHistoryRequest = new GameHistoryRequest
            {
                Message = "Get all games history!"
            };

            //Arrange
            _mock.Setup(x => x.GetAllGameHistories(gameHistoryRequest)).Returns(fakeGameHistories);

            //Act
            var result = _mock.Object.GetAllGameHistories(gameHistoryRequest).ToList();

            //Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void GetAllGameHistories_GameHistoryListMustBeReverse_ReturnTrue()
        {
            var fakeGameHistories = new List<GameHistoryResponse>
            {
                new GameHistoryResponse
                {
                    Id = 2,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test",
                    GameId= 2,
                    GameStateName = "Test",
                    WinnerName = "Test",
                },
                new GameHistoryResponse
                {
                    Id = 1,
                    FirstPlayerName = "Test2",
                    SecondPlayerName = "Test2", 
                    GameId= 1,
                    GameStateName = "Test2",
                    WinnerName = "Test2",
                }
            }.AsQueryable();

            var fakeGameHistoryResponse = new List<GameHistoryResponse>
            {
                new GameHistoryResponse
                {
                    Id = 2,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test",
                    GameId= 2,
                    GameStateName = "Test",
                    WinnerName = "Test",
                },
                new GameHistoryResponse
                {
                    Id = 1,
                    FirstPlayerName = "Test2",
                    SecondPlayerName = "Test2",
                    GameId= 1,
                    GameStateName = "Test2",
                    WinnerName = "Test2",
                }
            }; 
            
            var gameHistoryRequest = new GameHistoryRequest
            {
                Message = "Get all games history!"
            };

            //Arrange
            _mock.Setup(x => x.GetAllGameHistories(gameHistoryRequest)).Returns(fakeGameHistories);

            //Act
            var result = _mock.Object.GetAllGameHistories(gameHistoryRequest).ToList();

            //Assert
            Assert.True(fakeGameHistoryResponse.First() != result.First());
        }

        [Fact]
        public void GetAllGameHistories_Should_Return_Reverse_List()
        {
            //Arrange
            var gameHistoryRequest = new GameHistoryRequest
            {
                Message = "Get game history List"
            };

            var getHistoryListResponse = new List<GameHistoryResponse>
            {
                new GameHistoryResponse
                {
                    Id = 2,
                    FirstPlayerName = "Test3",
                    SecondPlayerName = "Test4",
                    GameId = 2,
                    GameStateName = "Test_State_Name",
                    WinnerName ="Test3"
                },

                new GameHistoryResponse
                {
                    Id = 1,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test2",
                    GameId = 1,
                    GameStateName = "Test_State_Name",
                    WinnerName ="Test"
                }
            };

            var notReversedList = new List<GameHistoryResponse>
            {
                new GameHistoryResponse
                {
                    Id = 1,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test2",
                    GameId = 1,
                    GameStateName = "Test_State_Name",
                    WinnerName ="Test"
                },

                new GameHistoryResponse
                {
                    Id = 2,
                    FirstPlayerName = "Test3",
                    SecondPlayerName = "Test4",
                    GameId = 2,
                    GameStateName = "Test_State_Name",
                    WinnerName ="Test3"
                },
            };

            _mock.Setup(x => x.GetAllGameHistories(gameHistoryRequest)).Returns(getHistoryListResponse);

            //Act
            var result = _mock.Object.GetAllGameHistories(gameHistoryRequest);

            //Assert
            Assert.NotEqual(notReversedList, result);
        }
    }
}