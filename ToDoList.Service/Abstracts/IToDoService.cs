using Core.Responses;
using Core.Services;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Models.Dtos.ToDos.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Abstracts;

public interface IToDoService : IService
{
    Task<ReturnModel<ToDoResponseDto>> AddAsync(CreateToDoRequest create);
    Task<ReturnModel<ToDoResponseDto>> UpdateAsync(UpdateToDoRequest update);
    Task<ReturnModel<ToDoResponseDto>> GetByIdAsync(Guid id);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllAsync();
    Task<ReturnModel<ToDoResponseDto>> DeleteAsync(Guid id);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByCategoryIdAsync(int id);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByTitleContainsAsync(string text);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByEndOfDateAsync();
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllByPriorityAsync(string text);
    Task<ReturnModel<List<ToDoResponseDto>>> GetAllCompletedAsync();
    Task<ReturnModel<List<ToDoResponseDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ReturnModel<List<ToDoResponseDto>>> GetTodayTasksAsync();

}
