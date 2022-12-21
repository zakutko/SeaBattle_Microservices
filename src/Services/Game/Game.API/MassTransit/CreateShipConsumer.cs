using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class CreateShipConsumer : IConsumer<CreateShipRequest>
    {
        private readonly IGameService _gameService;

        public CreateShipConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<CreateShipRequest> context)
        {
            var result = _gameService.CreateShipOnField(context.Message);
            await context.RespondAsync(result);
        }
    }
}
