using GameHistory.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;
using SeaBattle.Contracts.Lists;

namespace GameHistory.API.MassTransit
{
    public class GetAllGameHistoriesConsumer : IConsumer<GameHistoryRequest>
    {
        private readonly IGameHistoryService _gameHistoryService;

        public GetAllGameHistoriesConsumer(IGameHistoryService gameHistoryService)
        {
            _gameHistoryService = gameHistoryService;
        }

        public async Task Consume(ConsumeContext<GameHistoryRequest> context)
        {
            var result = _gameHistoryService.GetAllGameHistories(context.Message);

            var gameHistoryResponseList = new GameHistoryResponseList { GameHistoriesResponseList = result };

            await context.RespondAsync(gameHistoryResponseList);
        }
    }
}