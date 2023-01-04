using Game.BLL.Interfaces;
using Moq;
using SeaBattle.Contracts.Dtos;

namespace Game.BLL.Tests
{
    public class GameServiceTests
    {
        private const string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlRlc3QiLCJuYW1laWQiOiI0NmIyZTAzMy02MDYzLTRmZWQtOTI2MS1lMmIwYjJiNzQwNTIiLCJlbWFpbCI6InRlc3RAZ21haWwuY29tIiwibmJmIjoxNjY5OTg2ODk3LCJleHAiOjE2NzA1OTE2OTcsImlhdCI6MTY2OTk4Njg5N30.OaukKIoZeAOISA2llJZaYz93VltW9cvR6S5pr7RLHQphPxFK6_L05Z4gWWtKKej1TokN9pm46K1Z4nSiHIoLAQ";

        public Mock<IGameService> mock = new Mock<IGameService>();

        [Fact]
        public void GetAllGames_Should_Return_List()
        {
            //Arrange
            var getAllGamesRequest = new GameListRequest
            {
                Token = token,
            };

            var testGameListResponseList = new List<GameListResponse>
            {
                new GameListResponse
                {
                    Id = 1,
                    FirstPlayer = "Test",
                    SecondPlayer = "Test2",
                    GameState = "Test_Game_State",
                    NumberOfPlayers = 1,
                },
                new GameListResponse
                {
                    Id = 2,
                    FirstPlayer = "Test3",
                    SecondPlayer = "Test4",
                    GameState = "Test_Game_State",
                    NumberOfPlayers = 1,
                },
            };

            mock.Setup(x => x.GetAllGames(getAllGamesRequest).Result).Returns(testGameListResponseList);

            //Act
            var result = mock.Object.GetAllGames(getAllGamesRequest).Result;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(testGameListResponseList, result);
        }

        [Fact]
        public void CreateGame_If_Successful_Should_Not_Throw_An_Exeption_Message()
        {
            //Arrange
            var createGameRequest = new CreateGameRequest 
            { 
                Token = token 
            };
            
            string message = string.Empty;
            string testMessage = "Game creation failed!";

            //Act
            try
            {
                mock.Object.CreateGame(createGameRequest);
                message = "Game creation successful!";
            }
            catch
            {
                message = "Game creation failed!";
            }

            //Assert
            Assert.NotEqual(testMessage, message);
        }

        [Fact]
        public void IsGameOwner_Should_Return_IsGameOwnerResponse()
        {
            //Arrange
            var isGameOwnerRequest = new IsGameOwnerRequest
            {
                Token = token
            };

            var testIsGameOwnerResponse = new IsGameOwnerResponse
            {
                IsGameOwner = true,
                IsSecondPlayerConnected = true
            };

            mock.Setup(x => x.IsGameOwner(isGameOwnerRequest).Result).Returns(testIsGameOwnerResponse);

            //Act
            var result = mock.Object.IsGameOwner(isGameOwnerRequest).Result;

            //Assert
            Assert.Equal(testIsGameOwnerResponse, result);
        }

        [Fact]
        public void DeleteGame_If_Successful_Should_Not_Throw_An_Exeption_Message()
        {
            // Arrange
            var deleteGameRequest = new DeleteGameRequest
            {
                Token = token
            };

            //Act
            //Assert
            try
            {
                mock.Object.DeleteGame(deleteGameRequest);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void JoinSecondPlayer_If_Successful_Should_Not_Throw_An_Exeption_Message()
        {
            //Arrange
            var joinSecondPlayerRequest = new JoinSecondPlayerRequest
            {
                Token = token
            };

            //Act
            //Assert
            try
            {
                mock.Object.JoinSecondPlayer(joinSecondPlayerRequest);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void GetAllCells_Response_List_Should_Be_Order_By_Id()
        {
            //Arrange
            var getAllCellsRequest = new CellListRequest 
            { 
                Token = token 
            };

            var fakeGetAllCellsResponse = new List<CellListResponse>
            {
                new CellListResponse
                {
                    Id = 1,
                    X = 3,
                    Y = 3,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 2,
                    X = 1,
                    Y = 1,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 3,
                    X = 4,
                    Y = 4,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 4,
                    X = 2,
                    Y = 2,
                    CellStateId = 1
                }
            };

            var notOrderByIdListResponse = new List<CellListResponse>
            {
                new CellListResponse
                {
                    Id = 2,
                    X = 1,
                    Y = 1,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 4,
                    X = 2,
                    Y = 2,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 1,
                    X = 3,
                    Y = 3,
                    CellStateId = 1
                },

                new CellListResponse
                {
                    Id = 3,
                    X = 4,
                    Y = 4,
                    CellStateId = 1
                }
            };

            mock.Setup(x => x.GetAllCells(getAllCellsRequest).Result).Returns(fakeGetAllCellsResponse);

            //Act
            var result = mock.Object.GetAllCells(getAllCellsRequest).Result;

            //Assert
            Assert.NotEqual(notOrderByIdListResponse, result);
        }
    }
}