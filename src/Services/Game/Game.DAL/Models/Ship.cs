using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Ship")]
    public class Ship
    {
        [Key]
        public int Id { get; set; }
        [Column("directionId")]
        public int DirectionId { get; set; }
        [Column("shipStateId")]
        public int ShipStateId { get; set; }
        [Column("shipSizeId")]
        public int ShipSizeId { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual ShipState ShipState { get; set; }
        public virtual ShipSize ShipSize { get; set; }
        public virtual ShipWrapper ShipWrapper { get; set; }
    }
}
