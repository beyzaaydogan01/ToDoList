using AutoMapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.Categories.Responses;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Models.Dtos.ToDos.Responses;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateToDoRequest, ToDo>();
        CreateMap<UpdateToDoRequest, ToDo>();
        CreateMap<ToDo, ToDoResponseDto>()
            .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category.Name));

        CreateMap<Category, CategoryResponseDto>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();
    }
}
