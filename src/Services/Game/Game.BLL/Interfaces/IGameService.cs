using SeaBattle.Contracts.Dtos;

namespace Game.BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<GameListResponse> GetAllGames(GameListRequest gameListRequest);
        void CreateGame(CreateGameRequest createGameRequest);
        IsGameOwnerResponse IsGameOwner(IsGameOwnerRequest isGameOwnerRequest);
        void DeleteGame(DeleteGameRequest deleteGameRequest);
        void JoinSecondPlayer(JoinSecondPlayerRequest joinSecondPlayerRequest);
        IEnumerable<CellListResponse> GetAllCells(CellListRequest cellListRequest);
        CreateShipResponse CreateShipOnField(CreateShipRequest createShipRequest);
        void SetPlayerReady(IsPlayerReadyRequest isPlayerReadyRequest);
        IsTwoPlayersReadyResponse IsTwoPlayersReady(IsTwoPlayersReadyRequest isTwoPlayersReadyRequest);
        IEnumerable<CellListResponseForSecondPlayer> GetAllCellForSecondPlayer(CellListRequestForSecondPlayer cellListRequestForSecondPlayer);
        ShootResponse Fire(ShootRequest shootRequest);
        HitResponse GetPriority(HitRequest hitRequest);
        IsEndOfTheGameResponse IsEndOfTheGame(IsEndOfTheGameRequest isEndOfTheGameRequest);
        ClearingDBResponse ClearingDB(ClearingDBRequest clearingDBRequest);

    }
}
