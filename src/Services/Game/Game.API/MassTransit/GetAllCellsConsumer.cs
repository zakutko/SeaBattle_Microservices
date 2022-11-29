using Game.BLL.Interfaces;
using MassTransit;
using SeaBattle.Contracts.Dtos;
using SeaBattle.Contracts.Lists;

namespace Game.API.MassTransit
{
    public class GetAllCellsConsumer : IConsumer<CellListRequest>
    {
        private readonly IGameService _gameService;

        public GetAllCellsConsumer(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Consume(ConsumeContext<CellListRequest> context)
        {
            var result = _gameService.GetAllCells(context.Message);
            var cellList = new CellListResponseList { CellListResponses = result };

            await context.RespondAsync(cellList);
        }
    }
}