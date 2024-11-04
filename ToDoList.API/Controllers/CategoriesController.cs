using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Concretes;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateCategoryRequest dto)
        {
            var result = await categoryService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryRequest dto)
        {
            var result = await categoryService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var result = await categoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id)
        {
            var result = await categoryService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
