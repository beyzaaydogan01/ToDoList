using AutoMapper;
using Core.Exceptions;
using Core.Responses;
using Microsoft.AspNetCore.Diagnostics;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Models.Dtos.ToDos.Responses;
using ToDoList.Models.Entities;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Rules;

namespace ToDoList.Service.Concretes;

public sealed class ToDoService(IToDoRepository toDoRepository,
    IMapper mapper,
    ToDoBusinessRules businessRules
    ) : IToDoService
{
    public async Task<ReturnModel<ToDoResponseDto>> AddAsync(CreateToDoRequest create)
    {
        try
        {
            ToDo createdToDo = mapper.Map<ToDo>(create);

            businessRules.ToDoEndDateMustBeValid(createdToDo.EndDate);

            await toDoRepository.AddAsync(createdToDo);
             
            ToDoResponseDto responseDto = mapper.Map<ToDoResponseDto>(createdToDo);

            return new ReturnModel<ToDoResponseDto>
            {
                Data = responseDto,
                Message = "Todo Eklendi.",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ReturnModel<ToDoResponseDto>> DeleteAsync(Guid id)
    {
        try
        {
            ToDo toDo = await toDoRepository.GetByIdAsync(id);
            businessRules.ToDoIsNullCheck(toDo);

            await toDoRepository.RemoveAsync(toDo);

            ToDoResponseDto responseDto = mapper.Map<ToDoResponseDto>(toDo);

            return new ReturnModel<ToDoResponseDto>
            {
                Data = responseDto,
                Message = "Todo silindi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }

    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllAsync()
    {
        try
        {
            List<ToDo> todos = await toDoRepository.GetAllAsync();

            List<ToDoResponseDto> responseDto = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDto,
                Message = "Veri listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllByCategoryIdAsync(int id)
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(x => x.CategoryId == id, false);
            List<ToDoResponseDto> responseDto = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDto,
                Message = "Veriler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllByEndOfDateAsync()
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(x => x.EndDate < DateTime.Now);

            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Tarihi geçmiş veriler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllByPriorityAsync(string text)
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(x => x.Priority.ToString().Equals(text, StringComparison.OrdinalIgnoreCase));

            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Veriler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllByTitleContainsAsync(string text)
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(t => t.Title.Contains(text, StringComparison.InvariantCultureIgnoreCase));

            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Veriler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllCompletedAsync()
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(t => t.Completed.Equals(true));
            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Tamamlanmış görevler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var todos = await toDoRepository.GetAllAsync(t => t.StartDate >= startDate && t.EndDate <= endDate);
            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Belirtilen tarih aralığındaki görevler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<List<ToDoResponseDto>>> GetTodayTasksAsync()
    {
        try
        {
            var today = DateTime.Today;
            var todos = await toDoRepository.GetAllAsync(t => t.StartDate.Date == today);
            List<ToDoResponseDto> responseDtos = mapper.Map<List<ToDoResponseDto>>(todos);

            return new ReturnModel<List<ToDoResponseDto>>
            {
                Data = responseDtos,
                Message = "Bugünkü görevler listelendi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    public async Task<ReturnModel<ToDoResponseDto>> GetByIdAsync(Guid id)
    {
        try
        {
            ToDo todo = await toDoRepository.GetByIdAsync(id);
            businessRules.ToDoIsNullCheck(todo);

            ToDoResponseDto responseDto = mapper.Map<ToDoResponseDto>(todo);

            return new ReturnModel<ToDoResponseDto>
            {
                Data = responseDto,
                Message = string.Empty,
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    public async Task<ReturnModel<ToDoResponseDto>> UpdateAsync(UpdateToDoRequest update)
    {
        try
        {
            ToDo todo = mapper.Map<ToDo>(update);
            businessRules.ToDoIsNullCheck(todo);
            businessRules.ToDoEndDateMustBeValid(todo.EndDate);

            await toDoRepository.UpdateAsync(todo);

            ToDoResponseDto responseDto = mapper.Map<ToDoResponseDto>(todo);

            return new ReturnModel<ToDoResponseDto>
            {
                Data = responseDto,
                Message = string.Empty,
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }
}
