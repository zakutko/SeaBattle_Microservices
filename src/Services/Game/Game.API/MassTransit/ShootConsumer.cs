using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class ShootConsumer : IConsumer<ShootRequest>
    {
        private readonly IGameService _gameService;

        public ShootConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<ShootRequest> context)
        {
            var result = await _gameService.Fire(context.Message);

            await context.RespondAsync(result);
        }
    }
}