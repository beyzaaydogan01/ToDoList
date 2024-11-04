using Core.Responses;
using ToDoList.Models.Dtos.Tokens.Responses;
using ToDoList.Models.Dtos.Users.Requests;

namespace ToDoList.Service.Abstracts;

public interface IAuthenticationService
{
    Task<ReturnModel<TokenResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<ReturnModel<TokenResponseDto>> RegisterAsync(RegisterRequestDto dto);
}