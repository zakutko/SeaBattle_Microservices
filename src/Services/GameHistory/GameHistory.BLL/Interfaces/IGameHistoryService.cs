using SeaBattle.Contracts.Dtos;

namespace GameHistory.BLL.Interfaces
{
    public interface IGameHistoryService
    {
        IEnumerable<GameHistoryResponse> GetAllGameHistories(GameHistoryRequest gameHistoryRequest);
        TopPlayersResponse GetTopPlayers(TopPlayersRequest topPlayersRequest);
    }
}