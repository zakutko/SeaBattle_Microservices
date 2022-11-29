using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class CreateGameConsumer : IConsumer<CreateGameRequest>
    {
        private readonly IGameService _gameService;

        public CreateGameConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<CreateGameRequest> context)
        {
            try
            {
                _gameService.CreateGame(context.Message);
                await context.RespondAsync(new CreateGameResponse { Message = "Game creation successful!" });
            }
            catch
            {
                await context.RespondAsync(new CreateGameResponse { Message = "Game creation failed!"});
            }
        }
    }
}
