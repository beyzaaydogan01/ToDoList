using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Service.Abstracts;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ToDosController(IToDoService toDoService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateToDoRequest dto)
        {
            var result = await toDoService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateToDoRequest dto)
        {
            var result = await toDoService.UpdateAsync(dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await toDoService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await toDoService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("getallByCategory")]
        public async Task<IActionResult> GetAllByCategoryIdAsync(int id)
        {
            var result = await toDoService.GetAllByCategoryIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getallByTitleContains")]
        public async Task<IActionResult> GetAllByTitleContainsAsync(string text)
        {
            var result = await toDoService.GetAllByTitleContainsAsync(text);
            return Ok(result);
        }

        [HttpGet("getallByEndDate")]
        public async Task<IActionResult> GetAllByEndOfDateAsync()
        {
            var result = await toDoService.GetAllByEndOfDateAsync();
            return Ok(result);
        }

        [HttpGet("getallByPriority")]
        public async Task<IActionResult> GetAllByPriorityAsync(string text)
        {
            var result = await toDoService.GetAllByPriorityAsync(text);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            var result = await toDoService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
