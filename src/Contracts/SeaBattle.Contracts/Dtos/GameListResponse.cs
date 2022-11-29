namespace SeaBattle.Contracts.Dtos
{
    public class GameListResponse
    {
        public int Id { get; set; }
        public string FirstPlayer { get; set; }
#nullable enable
        public string? SecondPlayer { get; set; }
#nullable disable
        public string GameState { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}
