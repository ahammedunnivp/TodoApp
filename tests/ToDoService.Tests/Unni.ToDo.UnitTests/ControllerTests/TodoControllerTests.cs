using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Unni.Todo.Application.DTOs;
using Unni.Todo.Application.Interfaces;
using Unni.Todo.WebAPI.Controllers;

namespace Unni.ToDo.Tests.ControllerTests
{
    public class TodoControllerTests
    {
        private readonly ToDoController _controller;
        private readonly Mock<ITodoService> _todoServiceMock = new Mock<ITodoService>();
        private readonly Mock<ILogger<ToDoController>> _loggerMock = new Mock<ILogger<ToDoController>>();

        public TodoControllerTests()
        {
            _controller = new ToDoController(_todoServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetById_ReturnsItem_When_ValidId()
        {
            var dto = new TodoItemDto
            {
                Id = 1,
                Title = "Modified Name",
                Category = "Work",
                Difficulty = 10,
                IsDone = true
            };
            _todoServiceMock.Setup(s => s.GetById(1)).Returns(dto);

            var result = _controller.GetById(1);

            var objResult = Assert.IsType<OkObjectResult>(result);
            var responseDto = Assert.IsAssignableFrom<TodoItemDto>(objResult.Value);

            Assert.Equal(dto.Id, responseDto.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_When_NoItem()
        {
            _todoServiceMock.Setup(s => s.GetById(1)).Returns(() => null);

            var result = _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Search_ReturnsListOfToDos()
        {
            var request = new GetTodoRequest
            {
                Filter = new ToDoFilter { Category = "Work", Difficulty = 1, IsDoneFilter = true },
                Pagination = new Pagination { Page = 1, PageSize = 3, IsSortAscending = true, SortField = "title" },
                IsFilter = true
            };
            var todoItems = new List<TodoItemDto>
            {
                new TodoItemDto { Id =1, Title = "First Task", Category = "Work", Difficulty = 1, IsDone = true},
                new TodoItemDto { Id =2, Title = "Second Task", Category = "Work", Difficulty = 1, IsDone = true},
            };
            var expectedDto = new PaginatedResponseDto<TodoItemDto>(1, 3, todoItems, 2);

            _todoServiceMock.Setup(s => s.Search(request)).Returns(expectedDto);

            var result = _controller.Search(request);

            var objResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsAssignableFrom<PaginatedResponseDto<TodoItemDto>>(objResult.Value);

            Assert.Equal(todoItems.Count, dto.Items.Count());

        }

        [Fact]
        public void Search_ReturnsNotFound_WhenSearchIsEmpty()
        {
            var request = new GetTodoRequest
            {
                Filter = new ToDoFilter { Category = "Work", Difficulty = 1, IsDoneFilter = true },
                Pagination = new Pagination { Page = 1, PageSize = 3, IsSortAscending = true, SortField = "title" },
                IsFilter = true
            };
            var todoItems = new List<TodoItemDto>
            {
            };
            var expectedDto = new PaginatedResponseDto<TodoItemDto>(1, 3, todoItems, 2);

            _todoServiceMock.Setup(s => s.Search(request)).Returns(expectedDto);

            var result = _controller.Search(request);

            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public void UpdateTodo_Returns_CreateAtActionResult_OnSuccess()
        {
            var dto = new TodoItemDto
            {
                Id = 1,
                Title = "Modified Name",
                Category = "Work",
                Difficulty = 10,
                IsDone = true
            };

            _todoServiceMock.Setup(s => s.UpdateToDoItem(1, dto)).Returns(dto);
            _todoServiceMock.Setup(s => s.GetById(1)).Returns(dto);

            var result = _controller.UpdateToDo(1, dto);

            var objResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDto = Assert.IsAssignableFrom<TodoItemDto>(objResult.Value);
        }

        [Fact]
        public void UpdateTodo_Returns_BadRequest_OnIdMismatch()
        {
            var dto = new TodoItemDto
            {
                Id = 1,
                Title = "Modified Name",
                Category = "Work",
                Difficulty = 10,
                IsDone = true
            };

            _todoServiceMock.Setup(s => s.UpdateToDoItem(1, dto)).Returns(dto);
            _todoServiceMock.Setup(s => s.GetById(1)).Returns(dto);

            var result = _controller.UpdateToDo(2, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateTodo_ReturnsNotFound_When_GivenToDo_IsNotPresent()
        {
            var dto = new TodoItemDto
            {
                Id = 1,
                Title = "Modified Name",
                Category = "Work",
                Difficulty = 10,
                IsDone = true
            };

            _todoServiceMock.Setup(s => s.UpdateToDoItem(1, dto)).Returns(dto);
            _todoServiceMock.Setup(s => s.GetById(1)).Returns(() => null);

            var result = _controller.UpdateToDo(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
