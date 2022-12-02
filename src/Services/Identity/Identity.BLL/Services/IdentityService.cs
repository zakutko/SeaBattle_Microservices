using Identity.BLL.Interfaces;
using Identity.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeaBattle.Contracts.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace Identity.BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public UserResponse GetCurrUser(GetCurrUserRequest getCurrUserRequest)
        {
            if (getCurrUserRequest.Token.IsNullOrEmpty())
            {
                throw new ArgumentNullException();
            }

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
                throw new NullReferenceException("User does not exist!");
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
