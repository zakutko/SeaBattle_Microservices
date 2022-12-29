using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class IsTwoPlayersReadyConsumer : IConsumer<IsTwoPlayersReadyRequest>
    {
        private readonly IGameService _gameService;

        public IsTwoPlayersReadyConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<IsTwoPlayersReadyRequest> context)
        {
                var result = await _gameService.IsTwoPlayersReady(context.Message);

                await context.RespondAsync(result);
        }
    }
}
