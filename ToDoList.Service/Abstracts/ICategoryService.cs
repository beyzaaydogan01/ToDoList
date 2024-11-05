using Core.Responses;
using Core.Services;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.Categories.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Abstracts;

public interface ICategoryService : IService
{
    Task<ReturnModel<CategoryResponseDto>> AddAsync(CreateCategoryRequest create);
    Task<ReturnModel<CategoryResponseDto>> UpdateAsync(UpdateCategoryRequest update);
    Task<ReturnModel<CategoryResponseDto>> GetByIdAsync(int id);
    Task<ReturnModel<List<CategoryResponseDto>>> GetAllAsync();
    Task<ReturnModel<CategoryResponseDto>> DeleteAsync(int id);
}
