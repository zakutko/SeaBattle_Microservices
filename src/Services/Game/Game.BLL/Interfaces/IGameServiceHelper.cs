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
        IEnumerable<Cell> GetCellList(int fieldId);
        IEnumerable<Ship> GetShipList(int fieldId);
        string GetSecondPlayerId(string playerId);
        IEnumerable<Cell> GetAllCells(string shipDirectionName, int shipSize, int X, int Y, int fieldId);
    }
}