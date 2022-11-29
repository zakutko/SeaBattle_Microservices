using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class ClearingDbConsumer : IConsumer<ClearingDBRequest>
    {
        private readonly IGameService _gameService;

        public ClearingDbConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<ClearingDBRequest> context)
        {
            var result = _gameService.ClearingDB(context.Message);

            await context.RespondAsync(result);
        }
    }
}
