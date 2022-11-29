namespace SeaBattle.Contracts.Dtos
{
    public class TopPlayersResponse
    {
#nullable enable
        public string? FirstPlacePlayer { get; set; }
        public string? SecondPlacePlayer { get; set; }
        public string? ThirdPlacePlayer { get; set; }
        public int? FirstPlaceNumberOfWins { get; set; }
        public int? SecondPlaceNumberOfWins { get; set; }
        public int? ThirdPlaceNumberOfWins { get; set; }
#nullable disable
    }
}