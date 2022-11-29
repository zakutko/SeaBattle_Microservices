using SeaBattle.Contracts.Dtos;

namespace Identity.BLL.Interfaces
{
    public interface IIdentityService
    {
        Task<UserResponse> Login(LoginRequest loginRequestDto);
        Task<UserResponse> Register(RegisterRequest registerRequestDto);
        UserResponse GetCurrUser(GetCurrUserRequest getCurrUserRequest);
    }
}