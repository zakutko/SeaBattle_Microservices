using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;
using static MassTransit.ValidationResultExtensions;

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
            try
            {
                _gameService.SetPlayerReady(context.Message);
                await context.RespondAsync(new IsPlayerReadyResponse { Message = "The player is ready!"});
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new IsPlayerReadyResponse { Message = ex.Message });
            }
        }
    }
}
