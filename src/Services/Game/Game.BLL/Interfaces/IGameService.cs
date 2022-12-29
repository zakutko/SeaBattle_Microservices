using SeaBattle.Contracts.Dtos;

namespace Game.BLL.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameListResponse>> GetAllGames(GameListRequest gameListRequest);
        Task CreateGame(CreateGameRequest createGameRequest);
        Task<IsGameOwnerResponse> IsGameOwner(IsGameOwnerRequest isGameOwnerRequest);
        Task DeleteGame(DeleteGameRequest deleteGameRequest);
        Task JoinSecondPlayer(JoinSecondPlayerRequest joinSecondPlayerRequest);
        Task<IEnumerable<CellListResponse>> GetAllCells(CellListRequest cellListRequest);
        Task<CreateShipResponse> CreateShipOnField(CreateShipRequest createShipRequest);
        Task<IsPlayerReadyResponse> SetPlayerReady(IsPlayerReadyRequest isPlayerReadyRequest);
        Task<IsTwoPlayersReadyResponse> IsTwoPlayersReady(IsTwoPlayersReadyRequest isTwoPlayersReadyRequest);
        Task<IEnumerable<CellListResponseForSecondPlayer>> GetAllCellForSecondPlayer(CellListRequestForSecondPlayer cellListRequestForSecondPlayer);
        Task<ShootResponse> Fire(ShootRequest shootRequest);
        Task<HitResponse> GetPriority(HitRequest hitRequest);
        Task<IsEndOfTheGameResponse> IsEndOfTheGame(IsEndOfTheGameRequest isEndOfTheGameRequest);
        Task<ClearingDBResponse> ClearingDB(ClearingDBRequest clearingDBRequest);
    }
}
