using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("ShipSize")]
    public class ShipSize
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("shipSizeName")]
        public string ShipSizeName { get; set; }
    }
}
