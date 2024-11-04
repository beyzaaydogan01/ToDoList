using Core.Responses;
using ToDoList.Models.Dtos.Tokens.Responses;
using ToDoList.Models.Dtos.Users.Requests;
using ToDoList.Service.Abstracts;

namespace ToDoList.Service.Concretes;

public class AuthenticationService(IUserService _userService, IJwtService _jwtService) : IAuthenticationService
{
    public async Task<ReturnModel<TokenResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userService.LoginAsync(dto);
        var tokenResponse = await _jwtService.CreateJwtTokenAsync(user);

        return new ReturnModel<TokenResponseDto>
        {
            Data = tokenResponse,
            Message = "Giriş başarılı",
            StatusCode = 200,
            Success = true
        };
    }

    public async Task<ReturnModel<TokenResponseDto>> RegisterAsync(RegisterRequestDto dto)
    {
        var user = await _userService.RegisterAsync(dto);
        var tokenResponse = await _jwtService.CreateJwtTokenAsync(user);

        return new ReturnModel<TokenResponseDto>
        {
            Data = tokenResponse,
            Message = "Kayıt işlemi başarılı",
            StatusCode = 200,
            Success = true
        };
    }
}
