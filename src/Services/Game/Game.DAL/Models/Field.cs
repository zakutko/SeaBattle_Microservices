using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Game.DAL.Models
{
    [Table("Field")]
    public class Field
    {
        [Key]
        public int Id { get; set; }
        [Column("size")]
        public int Size { get; set; }
        [Column("appUserId")]
        public string PlayerId { get; set; }
        public virtual AppUser Player { get; set; }
    }
}
