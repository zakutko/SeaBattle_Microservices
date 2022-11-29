using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.Contracts.Dtos;
using SeaBattle.Contracts.Lists;

namespace SeaBattle.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IBus _bus;
        public GameController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameListResponse>>> GetAllGames(string token)
        {
            try
            {
                var gameListRequest = new GameListRequest { Token = token };
                var response = await _bus.Request<GameListRequest, GameListResponseList>(gameListRequest);
                return Ok(response.Message.GameListResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("createGame")]
        public async Task<ActionResult<CreateGameResponse>> CreateGame(string token)
        {
            try
            {
                var request = new CreateGameRequest { Token = token };
                var response = await _bus.Request<CreateGameRequest, CreateGameResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("isGameOwner")]
        public async Task<ActionResult<IsGameOwnerResponse>> IsGameOwner(string token)
        {
            try
            {
                var request = new IsGameOwnerRequest { Token = token };
                var response = await _bus.Request<IsGameOwnerRequest, IsGameOwnerResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("deleteGame")]
        public async Task<ActionResult<DeleteGameResponse>> DeleteGame(string token)
        {
            try
            {
                var request = new DeleteGameRequest { Token = token };
                var response = await _bus.Request<DeleteGameRequest, DeleteGameResponse>(request);
                return Ok(response.Message);
            }
            catch
            {
                return BadRequest(new DeleteGameResponse { Message = "Delete game was not successful!" });
            }
        }

        [HttpGet("joinSecondPlayer")]
        public async Task<ActionResult<JoinSecondPlayerResponse>> JoinSecondPlayer(int gameId, string token)
        {
            try
            {
                var request = new JoinSecondPlayerRequest { GameId = gameId, Token = token };
                var response = await _bus.Request<JoinSecondPlayerRequest, JoinSecondPlayerResponse>(request);
                return Ok(response.Message);
            }
            catch
            {
                return BadRequest(new JoinSecondPlayerResponse { Message = "Join second player was not successful!" });
            }
        }

        [HttpGet("prepareGame")]
        public async Task<ActionResult<IEnumerable<CellListRequest>>> GetGetCells(string token)
        {
            try
            {
                var gameListRequest = new CellListRequest { Token = token };
                var response = await _bus.Request<CellListRequest, CellListResponseList>(gameListRequest);
                return Ok(response.Message.CellListResponses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("prepareGame/createShipOnField")]
        public async Task<ActionResult<CreateShipResponse>> CreateShipOnField(CreateShipRequest request)
        {
            try
            {
                var response = await _bus.Request<CreateShipRequest, CreateShipResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("isPlayerReady")]
        public async Task<ActionResult<IsPlayerReadyResponse>> IsPlayerReady(string token)
        {
            try
            {
                var request = new IsPlayerReadyRequest { Token = token };
                var response = await _bus.Request<IsPlayerReadyRequest, IsPlayerReadyResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("isTwoPlayersReady")]
        public async Task<ActionResult<IsTwoPlayersReadyResponse>> IsTwoPlayersReady(string token)
        {
            try
            {
                var request = new IsTwoPlayersReadyRequest { Token = token };
                var response = await _bus.Request<IsTwoPlayersReadyRequest, IsTwoPlayersReadyResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/secondPlayerCells")]
        public async Task<ActionResult<IEnumerable<CellListResponseForSecondPlayer>>> GetAllCellForSecondPlayer(string token)
        {
            try
            {
                var request = new CellListRequestForSecondPlayer { Token = token };
                var response = await _bus.Request<CellListRequestForSecondPlayer, CellListResponseForSecondPlayerList>(request);
                return Ok(response.Message.CellListResponseForSecondPlayersList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("game/fire")]
        public async Task<ActionResult<ShootResponse>> Fire(ShootRequest shootRequest)
        {
            try
            {
                var response = await _bus.Request<ShootRequest, ShootResponse>(shootRequest);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/priority")]
        public async Task<ActionResult<HitResponse>> GetPriority(string token)
        {
            try
            {
                var request = new HitRequest { Token = token };
                var response = await _bus.Request<HitRequest, HitResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/endOfTheGame")]
        public async Task<ActionResult<IsEndOfTheGameResponse>> IsEndOfTheGame(string token)
        {
            try
            {
                var request = new IsEndOfTheGameRequest { Token = token };
                var response = await _bus.Request<IsEndOfTheGameRequest, IsEndOfTheGameResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/clearingDb")]
        public async Task<ActionResult<ClearingDBResponse>> ClearingDb(string token)
        {
            try
            {
                var request = new ClearingDBRequest { Token = token };
                var response = await _bus.Request<ClearingDBRequest, ClearingDBResponse>(request);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
    }
}