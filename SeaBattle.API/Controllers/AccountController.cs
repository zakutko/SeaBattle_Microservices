using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.Contracts.Dtos;

namespace SeaBattle.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IBus _bus;

        public AccountController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetCurrentUser(string token)
        {
            try
            {
                var request = new GetCurrUserRequest { Token = token };
                var response = await _bus.Request<GetCurrUserRequest, UserResponse>(request);

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            try
            {
                var response = await _bus.Request<LoginRequest, UserResponse>(loginRequest);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest registerRequest)
        {
            try
            {
                var response = await _bus.Request<RegisterRequest, UserResponse>(registerRequest);
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}