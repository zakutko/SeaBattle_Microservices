using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class IsPlayerReadyConsumer : IConsumer<IsPlayerReadyRequest>
    {
        private readonly IGameService _gameService;

        public IsPlayerReadyConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<IsPlayerReadyRequest> context)
        {
                var result = await _gameService.SetPlayerReady(context.Message);
                await context.RespondAsync(result);
        }
    }
}
