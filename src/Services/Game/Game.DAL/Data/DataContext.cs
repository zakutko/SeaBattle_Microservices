using Game.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Game.DAL.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<CellState> CellStates { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Models.Game> Games { get; set; }
        public DbSet<GameField> GameFields { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<ShipSize> ShipSizes { get; set; }
        public DbSet<ShipState> ShipStates { get; set; }
        public DbSet<ShipWrapper> ShipWrappers { get; set; }
    }
}