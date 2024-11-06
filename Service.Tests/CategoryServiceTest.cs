using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Dtos.Categories.Requests;
using ToDoList.Models.Dtos.Categories.Responses;
using ToDoList.Models.Entities;
using ToDoList.Service.Concretes;
using ToDoList.Service.Rules;
using Core.Exceptions;
using Core.Responses;
using Azure;

namespace ToDoList.Tests.Service
{
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<CategoryBusinessRules> _mockBusinessRules;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockBusinessRules = new Mock<CategoryBusinessRules>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object, _mockBusinessRules.Object);
        }

        [Test]
        public async Task AddAsync_WhenCategoryIsValid_ReturnSuccess()
        {
            // Arrange
            CreateCategoryRequest createRequest = new CreateCategoryRequest ("deneme");
            Category category = new Category 
            { 
               Name = "deneme" 
            };
            CategoryResponseDto responseDto = new CategoryResponseDto 
            { 
                Id = 1,
                Name = "deneme"
            };

            _mockMapper.Setup(m => m.Map<Category>(createRequest)).Returns(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(responseDto);
            _mockCategoryRepository.Setup(r => r.AddAsync(category)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.AddAsync(createRequest);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kategori eklendi.", result.Message);
            Assert.AreEqual(responseDto, result.Data);
        }

        [Test]
        public void AddAsync_WhenCategoryIsNull_ThrowNotFoundException()
        {
            // Arrange
            CreateCategoryRequest createRequest = new CreateCategoryRequest("deneme");
            _mockMapper.Setup(m => m.Map<Category>(createRequest)).Returns((Category)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _categoryService.AddAsync(createRequest));
        }

        [Test]
        public async Task GetByIdAsync_WhenCategoryExists_ReturnSuccess()
        {
            // Arrange
            var id = 1;
            var category = new Category 
            {
                Name = "deneme"
            };
            var responseDto = new CategoryResponseDto 
            {
                Id = 1,
                Name = "deneme"
            };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(responseDto);

            // Act
            var result = await _categoryService.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kategori getirildi.", result.Message);
            Assert.AreEqual(responseDto, result.Data);
        }

        [Test]
        public void GetByIdAsync_WhenCategoryDoesNotExist_ThrowNotFoundException()
        {
            // Arrange
            var id = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Category)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetByIdAsync(id));
        }

        [Test]
        public async Task DeleteAsync_WhenCategoryExists_ReturnSuccess()
        {
            // Arrange
            var id = 1;
            var category = new Category 
            {
                Name = "deneme"
            };
            var responseDto = new CategoryResponseDto 
            {
                Id = 1,
                Name = "deneme"
            };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(responseDto);
            _mockCategoryRepository.Setup(r => r.RemoveAsync(category)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.DeleteAsync(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Category deleted", result.Message);
            Assert.AreEqual(responseDto, result.Data);
        }

        [Test]
        public async Task GetAllAsync_ReturnAllCategories()
        {
            // Arrange
            List<Category> categories = new List<Category>();
            List<CategoryResponseDto> responseDtos = new();

            _mockCategoryRepository.Setup(r => r.GetAllAsync(null, true)).ReturnsAsync(categories);
            _mockMapper.Setup(m => m.Map<List<CategoryResponseDto>>(categories)).Returns(responseDtos);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kategoriler getirildi", result.Message);
            Assert.AreEqual(responseDtos, result.Data);
            Assert.AreEqual(200, result.StatusCode);

        }

        [Test]
        public async Task UpdateAsync_WhenCategoryIsUpdated_ReturnSuccess()
        {
            // Arrange
            UpdateCategoryRequest updateRequest = new UpdateCategoryRequest ( "deneme" );
            Category category = new Category 
            {
                Name = "deneme"
            };
            var responseDto = new CategoryResponseDto 
            {
                Id = 1,
                Name = "deneme"
            };

            _mockMapper.Setup(m => m.Map<Category>(updateRequest)).Returns(category);
            _mockCategoryRepository.Setup(r => r.UpdateAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryResponseDto>(category)).Returns(responseDto);

            // Act
            var result = await _categoryService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Kategori güncellendi.", result.Message);
            Assert.AreEqual(responseDto, result.Data);
        }
    }
}