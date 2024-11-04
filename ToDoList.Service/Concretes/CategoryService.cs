using AutoMapper;
using Core.Exceptions;
using Core.Responses;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.Categories.Responses;
using ToDoList.Models.Entities;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Rules;

namespace ToDoList.Service.Concretes;

public class CategoryService(ICategoryRepository categoryRepository,
    IMapper mapper,
    CategoryBusinessRules businessRules) : ICategoryService
{
    public async Task<ReturnModel<CategoryResponseDto>> AddAsync(CreateCategoryRequest create)
    {
        try
        {
            Category category = mapper.Map<Category>(create);
            businessRules.CategoryIsNullCheck(category);

            await categoryRepository.AddAsync(category);

            CategoryResponseDto userResponseDto = mapper.Map<CategoryResponseDto>(category);

            return new ReturnModel<CategoryResponseDto>
            {
                Data = userResponseDto,
                Message = "Kategori eklendi.",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> DeleteAsync(int id)
    {
        try
        {
            Category category = await categoryRepository.GetByIdAsync(id);
            businessRules.CategoryIsNullCheck(category);

            await categoryRepository.RemoveAsync(category);

            CategoryResponseDto categoryResponseDto = mapper.Map<CategoryResponseDto>(category);

            return new ReturnModel<CategoryResponseDto>
            {
                Data = categoryResponseDto,
                Message = "Category deleted",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ReturnModel<List<CategoryResponseDto>>> GetAllAsync()
    {
        try
        {
            List<Category> categories = await categoryRepository.GetAllAsync();

            List<CategoryResponseDto> responseDtos = mapper.Map<List<CategoryResponseDto>>(categories);

            return new ReturnModel<List<CategoryResponseDto>>
            {
                Data = responseDtos,
                Message = "Kategoriler getirildi",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            Category category = await categoryRepository.GetByIdAsync(id);
            businessRules.CategoryIsNullCheck(category);

            CategoryResponseDto responseDto = mapper.Map<CategoryResponseDto>(category);

            return new ReturnModel<CategoryResponseDto>
            {
                Data = responseDto,
                Message = "Kategori getirildi.",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    public async Task<ReturnModel<CategoryResponseDto>> UpdateAsync(UpdateCategoryRequest update)
    {
        try
        {
            Category category = mapper.Map<Category>(update);
            businessRules.CategoryIsNullCheck(category);

            await categoryRepository.UpdateAsync(category);

            CategoryResponseDto responseDto = mapper.Map<CategoryResponseDto>(category);

            return new ReturnModel<CategoryResponseDto>
            {
                Data = responseDto,
                Message = "Kategori güncellendi.",
                StatusCode = 200,
                Success = true
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
