using Core.Services;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.Categories.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Abstracts;

public interface ICategoryService : IService<Category, int, CategoryResponseDto, CreateCategoryRequest, UpdateCategoryRequest>
{
}
