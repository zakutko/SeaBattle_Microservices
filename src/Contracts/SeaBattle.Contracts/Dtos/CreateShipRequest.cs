using System.ComponentModel.DataAnnotations;

namespace SeaBattle.Contracts.Dtos
{
    public class CreateShipRequest
    {
        [Required]
        public int ShipDirection { get; set; }
        [Required]
        public int ShipSize { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
    }
}