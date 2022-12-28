using Game.DAL.Models;

namespace Game.BLL.Interfaces
{
    public interface IGameServiceHelper
    {
        IEnumerable<Cell> SetDafaultCells();
        Cell CreateNewCell(int id, int x, int y, int cellStateId, bool isDestroyed);
        bool CheckIsCellsWithStateBusyOrHit(IEnumerable<Cell>? cells, int gameStateId);
        string GetUsernameByDecodingJwtToken(string token);
        AppUser CreateNewAppUser(AppUser appUser, bool? isHit);
        Task<IEnumerable<Cell>> GetCellList(int fieldId);
        Task<IEnumerable<Ship>> GetShipList(int fieldId);
        Task<string> GetSecondPlayerId(string playerId);
        Task<IEnumerable<Cell>> GetAllCells(string shipDirectionName, int shipSize, int X, int Y, int fieldId);
    }
}