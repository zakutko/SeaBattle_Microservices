using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Position")]
    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Column("shipWrapperId")]
        public int ShipWrapperId { get; set; }
        [Column("cellId")]
        public int CellId { get; set; }
        public ShipWrapper ShipWrapper { get; set; }
        public Cell Cell { get; set; }
    }
}
