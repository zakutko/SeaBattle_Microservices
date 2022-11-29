using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;
using SeaBattle.Contracts.Lists;

namespace Game.API.MassTransit
{
    public class GetAllCellsForSecondPlayerConsumer : IConsumer<CellListRequestForSecondPlayer>
    {
        private readonly IGameService _gameService;

        public GetAllCellsForSecondPlayerConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<CellListRequestForSecondPlayer> context)
        {
            var result = _gameService.GetAllCellForSecondPlayer(context.Message);
            var gameList = new CellListResponseForSecondPlayerList { CellListResponseForSecondPlayersList = result };

            await context.RespondAsync(gameList);
        }
    }
}
