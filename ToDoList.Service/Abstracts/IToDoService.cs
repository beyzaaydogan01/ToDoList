using Core.Responses;
using Core.Services;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Models.Dtos.ToDos.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Abstracts;

public interface IToDoService : IService<ToDo, Guid, ToDoResponseDto, CreateToDoRequest, UpdateToDoRequest>
{
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByCategoryIdAsync(int id);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByTitleContainsAsync(string text);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByEndOfDateAsync();
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByPriorityAsync(string text);

}
