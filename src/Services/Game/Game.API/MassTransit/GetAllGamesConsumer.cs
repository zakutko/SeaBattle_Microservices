using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;
using SeaBattle.Contracts.Lists;

namespace Game.API.MassTransit
{
    public class GetAllGamesConsumer : IConsumer<GameListRequest>
    {
        private readonly IGameService _gameService;

        public GetAllGamesConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<GameListRequest> context)
        {
            var result = await _gameService.GetAllGames(context.Message);
            var gameList = new GameListResponseList { GameListResponses = result };

            await context.RespondAsync(gameList);
        }
    }
}