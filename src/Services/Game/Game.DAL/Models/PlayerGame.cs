using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("PlayerGame")]
    public class PlayerGame
    {
        [Key]
        public int Id { get; set; }
        [Column("gameId")]
        public int GameId { get; set; }
        [Column("firstPlayerId")]
        public string? FirstPlayerId { get; set; }
        [Column("secondPlayerId")]
        public string? SecondPlayerId { get; set; }
        [Column("isReadyFirstPlayer")]
        public bool? IsReadyFirstPlayer { get; set; }
        [Column("isReadySecondPlayer")]
        public bool? IsReadySecondPlayer { get; set; }
        public virtual AppUser FirstPlayer { get; set; }
        public virtual AppUser SecondPlayer { get; set; }
        public virtual Game Game { get; set; }
    }
}
