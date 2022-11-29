using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("ShipState")]
    public class ShipState
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("shipStateName")]
        public string ShipStateName { get; set; }
    }
}
