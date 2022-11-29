using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Cell")]
    public class Cell
    {
        [Key]
        public int Id { get; set; }
        [Required, Column("X")]
        public int X { get; set; }
        [Required, Column("Y")]
        public int Y { get; set; }
        [Column("cellStateId")]
        public int CellStateId { get; set; }
        public virtual CellState State { get; set; }
    }
}
