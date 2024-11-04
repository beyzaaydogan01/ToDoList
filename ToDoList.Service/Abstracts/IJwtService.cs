using ToDoList.Models.Dtos.Tokens.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Abstracts;

public interface IJwtService
{
    Task<TokenResponseDto> CreateJwtTokenAsync(User user);
}
