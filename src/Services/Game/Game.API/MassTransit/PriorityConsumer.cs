using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class PriorityConsumer : IConsumer<HitRequest>
    {
        private readonly IGameService _gameService;

        public PriorityConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<HitRequest> context)
        {
            var result = await _gameService.GetPriority(context.Message);

            await context.RespondAsync(result);
        }
    }
}
