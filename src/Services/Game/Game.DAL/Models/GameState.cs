using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("GameState")]
    public class GameState
    {
        [Key, ForeignKey("Game")]
        public int Id { get; set; }
        [Column("gameStateName")]
        public string GameStateName { get; set; }
    }
}
