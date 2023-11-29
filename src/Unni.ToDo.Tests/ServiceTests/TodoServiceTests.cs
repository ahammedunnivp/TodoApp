using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unni.ToDo.API.Data.Models;
using Unni.ToDo.API.Data.Repositories;
using Unni.ToDo.API.Data.UnitOfWork;
using Unni.ToDo.API.DTOs;
using Unni.ToDo.API.Enums;
using Unni.ToDo.API.Services;

namespace Unni.ToDo.Tests.ServiceTests
{
    public class TodoServiceTests
    {
        private readonly TodoService _sut;
        private readonly Mock<ITodoRepository> _todoRepoMock = new Mock<ITodoRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ITodoUnitOfWork> _uowMock = new Mock<ITodoUnitOfWork>();

        private TodoItemDto GetValidDto(int?id=null, string? title="Sample", string? category="Work")
        {
            return new TodoItemDto
            {
                Id = id,
                Title = title,
                Category = category,
                Description = "Sample text",
                Difficulty = 5,
                IsDone = true
            };
        }

        private TodoItemEntity GetValidEntity(int?id=null, string? title = "Sample", string? category = "Work")
        {
            return new TodoItemEntity
            {
                Id = id,
                Title = title,
                Category = category,
                Description = "Sample text",
                Difficulty = 5,
                IsDone = true
            };
        }

        public TodoServiceTests()
        {
            _sut = new TodoService(_todoRepoMock.Object, _mapperMock.Object, _uowMock.Object);
            _uowMock.Setup(x => x.SaveChanges());
        }

        [Fact]
        public void AddToDoItem_Returns_Item_OnSuccess()
        {
            var req = new CreateTodoRequest 
            { 
                Title = "Sample", Category = "Work", Description = "Sample text", Difficulty = 5
            };
            var id = 1;
            var entity = this.GetValidEntity(id);
            var dto = this.GetValidDto(id);


            _mapperMock.Setup(mapper => mapper.Map<TodoItemEntity>(req)).Returns(entity);
            _mapperMock.Setup(mapper => mapper.Map<TodoItemDto>(entity)).Returns(dto);
            _todoRepoMock.Setup(r => r.Add(entity));

            var response = _sut.AddToDoItem(req);
            Assert.IsType<TodoItemDto>(response);
            Assert.Equal(id, response.Id);
        }

        [Fact]
        public void DeleteToDoItem_Verify_DeleteIsSaved()
        {
            var id = 1;

            _todoRepoMock.Setup(r => r.Delete(id));

            _sut.DeleteToDoItemById(id);


            _todoRepoMock.Verify(r => r.Delete(id), Times.Once);
            _uowMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void GetById_ShouldReturnTodo_WhenTodoExists()
        {
            var id = 10;
            var entity = this.GetValidEntity(id);
            var expectedDto = this.GetValidDto(id);


            _mapperMock.Setup(mapper => mapper.Map<TodoItemDto>(entity)).Returns(expectedDto);
            _todoRepoMock.Setup(x => x.GetById(id)).Returns(entity);



            var resultDto = _sut.GetById(id);

            Assert.Equal(id, resultDto.Id);
            Assert.Equal(expectedDto.Title, resultDto.Title);
        }

        [Fact]
        public void GetById_ShouldReturnNothing_WhenTodoDoesnotExists()
        {
            var id = 10;
            var entity = this.GetValidEntity(id);
            var expectedDto = this.GetValidDto(id);

            _mapperMock.Setup(mapper => mapper.Map<TodoItemDto>(entity)).Returns(expectedDto);
            _todoRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null);


            var resultDto = _sut.GetById(id);

            Assert.Null(resultDto);
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(2, 10)]
        public void GetAllTodos_Should_ReturnMappedTodos(int page, int pageSize)
        {
            // Arrange
            var pagination = new Pagination { Page = page, PageSize = pageSize };
            
            var todoEntities = new List<TodoItemEntity>
            {
                this.GetValidEntity(1, "First"),
                this.GetValidEntity(2, "Second"),
            };

            var filter = new ToDoFilter
            {
                Difficulty = 5,
                IsDoneFilter = true
            };

            var responseDtos = new List<TodoItemDto>
            {
               this.GetValidDto(1, "First"),
               this.GetValidDto(2, "Second")
            };

            var getRequestDto = new GetTodoRequest
            {
                IsFilter = true,
                Filter = filter,
                Pagination = pagination
            };

            _todoRepoMock.Setup(repo => repo.Search(pagination, filter)).Returns((todoEntities, 2));
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<TodoItemDto>>(todoEntities)).Returns(responseDtos);

            var expectedDtos = new PaginatedResponseDto<TodoItemDto>(pagination, responseDtos, 2);


            var resultDtos = _sut.Search(getRequestDto);

            // Assert
            Assert.Equal(2, resultDtos?.Items.Count());
            Assert.Equal(page, resultDtos?.Pagination?.Page);
            Assert.Equal(pageSize, resultDtos?.Pagination?.PageSize);
            Assert.IsType<PaginatedResponseDto<TodoItemDto>>(resultDtos);
        }

        [Theory]
        [InlineData(1, 10000)]
        [InlineData(1, 100)]
        public void GetAllTodos_DefaultPagination_IsApplied_WhenPageSizeIsHuge(int page, int pageSize)
        {
            var maxPageSize = 60;
            // Arrange
            var pagination = new Pagination { Page = page, PageSize = pageSize };

            var todoEntities = new List<TodoItemEntity>
            {
                this.GetValidEntity(1, "First"),
                this.GetValidEntity(2, "Second"),
            };

            var filter = new ToDoFilter
            {
                Difficulty = 5,
                IsDoneFilter = true
            };

            var responseDtos = new List<TodoItemDto>
            {
               this.GetValidDto(1, "First"),
               this.GetValidDto(2, "Second")
            };

            var getRequestDto = new GetTodoRequest
            {
                IsFilter = true,
                Filter = filter,
                Pagination = pagination
            };

            _todoRepoMock.Setup(repo => repo.Search(pagination, filter)).Returns((todoEntities, 2));
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<TodoItemDto>>(todoEntities)).Returns(responseDtos);

            var expectedDtos = new PaginatedResponseDto<TodoItemDto>(pagination, responseDtos, 2);


            var resultDtos = _sut.Search(getRequestDto);

            // Assert
            Assert.Equal(2, resultDtos?.Items.Count());
            Assert.Equal(page, resultDtos?.Pagination?.Page);
            Assert.Equal(maxPageSize, resultDtos?.Pagination?.PageSize);
            Assert.IsType<PaginatedResponseDto<TodoItemDto>>(resultDtos);
        }

        [Theory]
        [InlineData(1, "New Title")]
        public void UpdateToDoItem_Returns_UpdatedItem_OnSuccess(int id, string upadtedTitle)
        {
            var dto = this.GetValidDto(id, upadtedTitle);
            var updatedItem = this.GetValidEntity(id, upadtedTitle);

            _mapperMock.Setup(mapper => mapper.Map<TodoItemDto>(updatedItem)).Returns(dto);
            _todoRepoMock.Setup(r => r.Update(updatedItem)).Returns(updatedItem);
            _todoRepoMock.Setup(r => r.GetById(id)).Returns(updatedItem);

            var response = _sut.UpdateToDoItem(id, dto);
            Assert.IsType<TodoItemDto>(response);
            Assert.Equal(upadtedTitle, response.Title);
        }
    }

}
