using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class IsGameOwnerConsumer : IConsumer<IsGameOwnerRequest>
    {
        private readonly IGameService _gameService;

        public IsGameOwnerConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<IsGameOwnerRequest> context)
        {
            var result = _gameService.IsGameOwner(context.Message);

            await context.RespondAsync(result);
        }
    }
}