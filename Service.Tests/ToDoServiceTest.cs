using AutoMapper;
using Core.Exceptions;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.ToDos.Requests;
using ToDoList.Models.Dtos.ToDos.Responses;
using ToDoList.Models.Entities;
using ToDoList.Models.Enums;
using ToDoList.Service.Concretes;
using ToDoList.Service.Rules;

namespace Service.Tests;

public class ToDoServiceTest
{
    private ToDoService toDoService;
    private Mock<IMapper> mockMapper;
    private Mock<IToDoRepository> repositoryMock;
    private Mock<ToDoBusinessRules> rulesMock;

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new Mock<IToDoRepository>();
        mockMapper = new Mock<IMapper>();
        rulesMock = new Mock<ToDoBusinessRules>();
        toDoService = new ToDoService(repositoryMock.Object, mockMapper.Object, rulesMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsSuccess()
    {
        // Arange
        List<ToDo> todos = new List<ToDo>();
        List<ToDoResponseDto> responses = new();
        repositoryMock.Setup(x => x.GetAllAsync(null, true)).ReturnsAsync(todos);
        mockMapper.Setup(x => x.Map<List<ToDoResponseDto>>(todos)).Returns(responses);

        // Act 

        var result = await toDoService.GetAllAsync();

        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual(responses, result.Data);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(string.Empty, result.Message);
    }

    [Test]
    public async Task Add_WhenToDoAdded_ReturnsSuccess()
    {
        // Arange
        CreateToDoRequest dto = new CreateToDoRequest("Deneme", "Content", "2024-01-01", "2024-01-05", 1, "High");
        ToDo todo = new ToDo
        {
            Id = new Guid("{E1D5A9A5-2A12-4767-8C55-67D8EBF084DB}"),
            Title = "Deneme",
            Description = "Görev", 
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = Priority.High, 
            CategoryId = 1, 
            Completed = false, 
            UserId = "{9A7E7F27-3D3C-48E4-93D7-C715B7E0F67F}",
            CreatedDate = DateTime.Now 
        };

        ToDoResponseDto response = new ToDoResponseDto
        {
            Id = new Guid("{E1D5A9A5-2A12-4767-8C55-67D8EBF084DB}"),
            Title = "Deneme",
            Description = "Deneme",
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = "High",
            Category = "Deneme",
            Completed = "Hayır" 
        };

        mockMapper.Setup(x => x.Map<ToDo>(dto)).Returns(todo);
        repositoryMock.Setup(x => x.AddAsync(todo)).ReturnsAsync(todo);
        mockMapper.Setup(x => x.Map<ToDoResponseDto>(todo)).Returns(response);


        // Act
        var result = await toDoService.AddAsync(dto);

        //Assert
        Assert.AreEqual(response, result.Data);
        Assert.IsTrue(result.Success);
    }

    public async Task GetById_WhenToDoIsNotPresent_ThrowsException()
    {
        // Arange 
        Guid id = new Guid("{D0A5C827-9C98-47E9-A5C7-B81576D48F6A}");
        ToDo todo = null;
        rulesMock.Setup(x => x.ToDoIsNullCheck(todo)).Throws(new NotFoundException("İlgili todo bulunamadı."));

        // Assert
        Assert.Throws<NotFoundException>(() => toDoService.GetByIdAsync(id), "İlgili todo bulunamadı.");
    }

    [Test]
    public async Task GetById_WhenToDoIsPresent_ReturnsSuccess()
    {
        ToDo todo = new ToDo
        {
            Id = new Guid("{D0A5C827-9C98-47E9-A5C7-B81576D48F6A}")
        };

        Guid id = new Guid("{D0A5C827-9C98-47E9-A5C7-B81576D48F6A}");

        ToDoResponseDto response = new ToDoResponseDto
        {
            Id = new Guid("{E1A4F7A6-6F48-4D7C-8E6E-739A871BF982}"),
            Title = "Deneme",
            Description = "Deneme",
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = "High",
            Category = "Deneme",
            Completed = "Hayır"
        };

        repositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(todo);
        rulesMock.Setup(x => x.ToDoIsNullCheck(todo));
        mockMapper.Setup(x => x.Map<ToDoResponseDto>(todo)).Returns(response);

        var result = await toDoService.GetByIdAsync(id);

        Assert.AreEqual(response, result.Data);
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task DeleteAsync_WhenToDoExists_ReturnsSuccess()
    {
        // Arrange

        ToDo todo = new ToDo
        {
            Id = new Guid("{BA663833-98D6-4BE6-93C3-65997006B13Z}")
        };

        Guid id = new Guid("{BA663833-98D6-4BE6-93C3-65997006B13Z}");

        ToDoResponseDto response = new ToDoResponseDto 
        { 
            Id = new Guid("{BA663833-98D6-4BE6-93C3-65997006B13Z}"),
            Title = "Deneme",
            Description = "Deneme",
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = "High",
            Category = "Deneme",
            Completed = "Hayır"
        };

        repositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(todo);
        rulesMock.Setup(x => x.ToDoIsNullCheck(todo));
        mockMapper.Setup(x => x.Map<ToDoResponseDto>(todo)).Returns(response);

        // Act
        var result = await toDoService.DeleteAsync(id);

        // Assert
        Assert.AreEqual(response, result.Data);
        Assert.IsTrue(result.Success);
        Assert.AreEqual("Todo silindi", result.Message);
    }

    [Test]
    public async Task UpdateAsync_WhenToDoUpdated_ReturnsSuccess()
    {
        // Arrange
        UpdateToDoRequest dto = new UpdateToDoRequest 
        (
            "Deneme",
            "Content",
            "2024-01-01",
            "2024-01-05",
            1,
            "High",
            "Yes"
        );

        ToDo todo = new ToDo {
            Title = "Deneme",
            Description = "Görev",
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = Priority.High,
            CategoryId = 1,
            Completed = false,
            UserId = "{5C95E9E2-3ECE-4465-8A1D-8E38CA2BFFDC}",
            CreatedDate = DateTime.Now
        };
        ToDoResponseDto response = new ToDoResponseDto 
        {
            Id = new Guid("{BT663833-98D6-4BE6-93C3-65997006B13Z}"),
            Title = "Deneme",
            Description = "Deneme",
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2024-01-05"),
            Priority = "High",
            Category = "Deneme",
            Completed = "Hayır"
        };

        mockMapper.Setup(x => x.Map<ToDo>(dto)).Returns(todo);
        repositoryMock.Setup(x => x.UpdateAsync(todo)).ReturnsAsync(todo);
        mockMapper.Setup(x => x.Map<ToDoResponseDto>(todo)).Returns(response);

        // Act
        var result = await toDoService.UpdateAsync(dto);

        // Assert
        Assert.AreEqual(response, result.Data);
        Assert.IsTrue(result.Success);
        Assert.AreEqual("Kategori güncellendi.", result.Message);
    }
}
