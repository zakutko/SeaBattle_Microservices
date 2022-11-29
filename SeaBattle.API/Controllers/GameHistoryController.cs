using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.Contracts.Lists;
using SeaBattle.Contracts.Dtos;

namespace SeaBattle.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class GameHistoryController : ControllerBase
    {
        private readonly IBus _bus;

        public GameHistoryController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameHistoryResponse>>> GetAllGameHistories()
        {
            try
            {
                var gameHistoryRequest = new GameHistoryRequest { Message = "Get all games history!" };
                var response = await _bus.Request<GameHistoryRequest, GameHistoryResponseList>(gameHistoryRequest);
                return Ok(response.Message.GameHistoriesResponseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("topPlayers")]
        public async Task<ActionResult<TopPlayersResponse>> GetTopPlayers()
        {
            try
            {
                var topPlayersRequest = new TopPlayersRequest { Message = "Get all games history!" };
                var response = await _bus.Request<TopPlayersRequest, TopPlayersResponse>(topPlayersRequest);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}