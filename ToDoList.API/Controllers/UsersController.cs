using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos.Users.Requests;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Concretes;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService,
        IAuthenticationService authenticationService) : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto dto)
        {
            var result = await authenticationService.RegisterAsync(dto);

            return Ok(result);
        }

        [HttpGet("email")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmailAsync([FromQuery] string email)
        {
            var result = await userService.GetByEmailAsync(email);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto dto)
        {
            var result = await authenticationService.LoginAsync(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromQuery] string id, [FromBody] UserUpdateRequestDto dto)
        {
            var result = await userService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync(string id, ChangePasswordRequestDto dto)
        {
            var result = await userService.ChangePasswordAsync(id, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string id)
        {
            var result = await userService.DeleteAsync(id);
            return Ok(result);
        }
    }
}

