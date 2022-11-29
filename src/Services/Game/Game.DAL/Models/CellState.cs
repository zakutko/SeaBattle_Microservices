using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("CellState")]
    public class CellState
    {
        [Key, ForeignKey("Cell")]
        public int Id { get; set; }
        [Column("cellStateName")]
        public string CellStateName { get; set; }
    }
}
