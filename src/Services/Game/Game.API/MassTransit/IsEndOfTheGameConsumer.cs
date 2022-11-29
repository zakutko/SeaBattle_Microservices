using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class IsEndOfTheGameConsumer : IConsumer<IsEndOfTheGameRequest>
    {
        private readonly IGameService _gameService;

        public IsEndOfTheGameConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<IsEndOfTheGameRequest> context)
        {
            var result = _gameService.IsEndOfTheGame(context.Message);

            await context.RespondAsync(result);
        }
    }
}
