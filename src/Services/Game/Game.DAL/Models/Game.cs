using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column("gameStateId")]
        public int GameStateId { get; set; }
        public virtual GameState GameState { get; set; }
    }
}
