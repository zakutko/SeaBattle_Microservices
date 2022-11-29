using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Direction")]
    public class Direction
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("directionName")]
        public string DirectionName { get; set; }
    }
}
