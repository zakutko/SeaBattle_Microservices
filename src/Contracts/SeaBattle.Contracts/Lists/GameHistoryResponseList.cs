using SeaBattle.Contracts.Dtos;

namespace SeaBattle.Contracts.Lists
{
    public class GameHistoryResponseList
    {
        public IEnumerable<GameHistoryResponse> GameHistoriesResponseList { get; set; }
    }
}