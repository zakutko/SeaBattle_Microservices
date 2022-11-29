using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("GameField")]
    public class GameField
    {
        [Key]
        public int Id { get; set; }
        [Column("gameId")]
        public int GameId { get; set; }
        [Column("firstFieldId")]
        public int? FirstFieldId { get; set; }
        [Column("secondFieldId")]
        public int? SecondFieldId { get; set; }
        public virtual Game Game { get; set; }
        public virtual Field FirstField { get; set; }
        public virtual Field SecondField { get; set; }
    }
}
