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
            await context.RespondAsync(await _gameService.IsGameOwner(context.Message));
        }
    }
}