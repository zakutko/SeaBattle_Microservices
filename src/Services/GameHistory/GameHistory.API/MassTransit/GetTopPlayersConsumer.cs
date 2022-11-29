using GameHistory.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace GameHistory.API.MassTransit
{
    public class GetTopPlayersConsumer : IConsumer<TopPlayersRequest>
    {
        private readonly IGameHistoryService _gameHistoryService;

        public GetTopPlayersConsumer(IGameHistoryService gameHistoryService)
        {
            _gameHistoryService = gameHistoryService;
        }

        public async Task Consume(ConsumeContext<TopPlayersRequest> context)
        {
            var result = _gameHistoryService.GetTopPlayers(context.Message);

            await context.RespondAsync(result);
        }
    }
}