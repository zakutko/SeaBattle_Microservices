using Identity.BLL.Interfaces;
using Identity.DAL.Interfaces;
using Identity.DAL.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeaBattle.Contracts.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace Identity.BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRepository<AppUser> _appUserRepository;

        public IdentityService(IRepository<AppUser> appUserRepository, IPublishEndpoint publishEndpoint, UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _publishEndpoint = publishEndpoint;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _appUserRepository = appUserRepository;
        }

        public UserResponse GetCurrUser(GetCurrUserRequest getCurrUserRequest)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(getCurrUserRequest.Token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            var response = new UserResponse
            {
                Username = username,
                Token = getCurrUserRequest.Token
            };

            return response;
        }

        public async Task<UserResponse> Login(LoginRequest loginRequestDto)
        {
            var user = _userManager.FindByEmailAsync(loginRequestDto.Email).Result;

            if (user == null)
            {
                throw new Exception("User does not exist!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);

            if (!result.Succeeded)
            {
                throw new Exception("Invalid password!");
            }

            var jwtToken = _tokenService.CreateToken(user);

            var response = new UserResponse()
            {
                Username = user.UserName,
                Token = jwtToken
            };

            await _publishEndpoint.Publish(response);
            return response;
        }

        public async Task<UserResponse> Register(RegisterRequest requestDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == requestDto.Email))
            {
                throw new Exception("Email taken!");
            }
            if (await _userManager.Users.AllAsync(x => x.UserName == requestDto.UserName))
            {
                throw new Exception("Username taken!");
            }

            var appUser = new AppUser
            {
                DisplayName = requestDto.DisplayName,
                Email = requestDto.Email,
                UserName = requestDto.UserName
            };

            var result = await _userManager.CreateAsync(appUser, requestDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Problem registering user!");
            }

            var jwtToken = _tokenService.CreateToken(appUser);
            return new UserResponse { Username = requestDto.UserName, Token = jwtToken };
        }
    }
}
