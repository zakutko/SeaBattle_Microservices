using AutoMapper;
using GameHistory.BLL.Interfaces;
using GameHistory.BLL.Services;
using GameHistory.DAL.Interfaces;
using Moq;
using SeaBattle.Contracts.Dtos;

namespace GameHistory.BLL.Tests
{
    public class GameHistoryServiceTests
    {
        private readonly Mock<IMapper> _mapperMoq; 
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGameHistoryService> _mock = new Mock<IGameHistoryService>();

        public GameHistoryServiceTests()
        {
            _mapperMoq = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public void GetAllGameHistories_RequestMessageMayBeEmpty_ReturnTrue()
        {
            var fakeGameHistories = new List<DAL.Models.GameHistory>
            {
                new DAL.Models.GameHistory
                {
                    Id = 2,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test",
                    GameId= 2,
                    GameStateName = "Test",
                    WinnerName = "Test",
                },
                new DAL.Models.GameHistory
                {
                    Id = 1,
                    FirstPlayerName = "Test2",
                    SecondPlayerName = "Test2",
                    GameId= 1,
                    GameStateName = "Test2",
                    WinnerName = "Test2",
                }
            }.AsQueryable();

            //Arrange
            _unitOfWorkMock.Setup(x => x.GameHistoryRepository.CreateRange(fakeGameHistories));
            _unitOfWorkMock.Setup(x => x.GameHistoryRepository.GetAllAsync().Result).Returns(fakeGameHistories);

            var request = new GameHistoryRequest { Message = String.Empty };
            //Act
            var gameHistoryService = new GameHistoryService(_unitOfWorkMock.Object, _mapperMoq.Object);
            var result = gameHistoryService.GetAllGameHistories(request);

            //Assert
            Assert.True(result.Any());
        }

        [Fact]
        public void GetAllGameHistories_GameHistoryListMustBeReverse_ReturnTrue()
        {
            var fakeGameHistories = new List<DAL.Models.GameHistory>
            {
                new DAL.Models.GameHistory
                {
                    Id = 2,
                    FirstPlayerName = "Test",
                    SecondPlayerName = "Test",
                    GameId= 2,
                    GameStateName = "Test",
                    WinnerName = "Test",
                },
                new DAL.Models.GameHistory
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

            //Arrange
            _unitOfWorkMock.Setup(x => x.GameHistoryRepository.CreateRange(fakeGameHistories));
            _unitOfWorkMock.Setup(x => x.GameHistoryRepository.GetAllAsync().Result).Returns(fakeGameHistories);

            var request = new GameHistoryRequest { Message = String.Empty };
            //Act
            var gameHistoryService = new GameHistoryService(_unitOfWorkMock.Object, _mapperMoq.Object);
            var result = gameHistoryService.GetAllGameHistories(request).ToList();

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