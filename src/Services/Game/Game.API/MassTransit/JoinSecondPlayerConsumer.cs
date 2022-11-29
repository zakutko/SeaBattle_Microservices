using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;

namespace Game.API.MassTransit
{
    public class JoinSecondPlayerConsumer : IConsumer<JoinSecondPlayerRequest>
    {
        private readonly IGameService _gameService;

        public JoinSecondPlayerConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<JoinSecondPlayerRequest> context)
        {
            try
            {
                _gameService.JoinSecondPlayer(context.Message);
                await context.RespondAsync(new JoinSecondPlayerResponse { Message = "Join second player was successful!" });
            }
            catch
            {
                await context.RespondAsync(new JoinSecondPlayerResponse { Message = "Join second player was not successful!" });
            }
        }
    }
}
