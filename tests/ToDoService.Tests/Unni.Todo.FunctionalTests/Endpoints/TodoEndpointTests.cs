using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Unni.Todo.Application.DTOs;
using Unni.Todo.FunctionalTests;
using Unni.Todo.WebAPI;

namespace Unni.Todo.FunctionalTests.Endpoints
{
    public class TodoEndpointTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient _client = factory.CreateClient();

        [Theory]
        [InlineData(2)]
        public async void GetTodoById_ShouldReturnTodoItem(int id)
        {
            var todo = await _client.GetFromJsonAsync<TodoItemDto>($"api/todo/{id}");

            Assert.Equal(id, todo?.Id.Value);
        }

        [Theory]
        [InlineData(50)]
        public async void GetTodoById_ShouldReturnNotFound_WhenNoItem(int id)
        {
            var resp = await _client.GetAsync($"api/todo/{id}");


            Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
        }

        [Theory]
        [InlineData("falseId")]
        public async void GetTodoById_ShouldReturnBadRequest_WhenIdIsNotInt(string id)
        {
            var resp = await _client.GetAsync($"api/todo/{id}");


            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Theory]
        [InlineData("New Task", "Newly added", "Work", 5)]
        public async void AddToDo_ShouldReturnTodoItem(string title, string description, string category, int difficulty)
        {
            var dto = new CreateTodoRequest
            {
                Title = title,
                Description = description,
                Category = category,
                Difficulty = difficulty
            };

            var resp = await _client.PostAsJsonAsync("api/todo", dto);

            resp.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, resp.StatusCode);
            var content = await resp.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<TodoItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(todo);
            Assert.Equal(title, todo.Title);

        }

        [Theory]
        [InlineData("New Task", "Newly added", "Work", 5)]
        public async void AddToDo_ItemAdded_SholdHaveNotDoneStatus(string title, string description, string category, int difficulty)
        {
            var dto = new CreateTodoRequest
            {
                Title = title,
                Description = description,
                Category = category,
                Difficulty = difficulty
            };

            var resp = await _client.PostAsJsonAsync("api/todo", dto);


            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<TodoItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(todo);
            Assert.False(todo.IsDone);
        }

        [Fact]
        public async void AddToDo_EmptyCreatedRequest_ReturnsBadRequest()
        {
            var dto = new CreateTodoRequest();

            var resp = await _client.PostAsJsonAsync("api/todo", dto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Theory]
        [InlineData("New Task", "Newly added", "Work", 5)]
        public async void ToDo_AddToDo_ReturnsBadRequest_IfMalformedRequest(string title, string description, string category, int difficulty)
        {
            var dto = new
            {
                NewTitle = title,
                Description = description,
                Category = category,
                Difficulty = difficulty
            };

            var resp = await _client.PostAsJsonAsync("api/todo", dto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }



        [Theory]
        [InlineData("New task without details")]
        public async void AddToDo_CreatedRequest_With_NullableData_ReturnsTodoItem(string title)
        {
            var dto = new CreateTodoRequest
            {
                Title = title
            };

            var resp = await _client.PostAsJsonAsync("api/todo", dto);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<TodoItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(todo);
            Assert.Equal(title, todo.Title);
        }


        [Theory]
        [InlineData(1, "Update Task", "updated added", "Work", 5, true)]
        public async void UpdateToDo_ShouldReturn_UpdatedItem(int id, string title, string description, string category, int difficulty, bool isDone)
        {
            var dto = new TodoItemDto
            {
                Id = id,
                Title = title,
                Description = description,
                Category = category,
                Difficulty = difficulty,
                IsDone = isDone
            };

            var resp = await _client.PutAsJsonAsync($"api/todo/{id}", dto);

            resp.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, resp.StatusCode);

            var content = await resp.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<TodoItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(todo);
            Assert.Equal(title, todo.Title);
            Assert.True(todo.IsDone);
        }

        [Theory]
        [InlineData(20, "Update Task", "updated added", "Work", 5, true)]
        public async void UpdateToDo_SholdReturn_NotFound_WhenNoItem(int id, string title, string description, string category, int difficulty, bool isDone)
        {
            var dto = new TodoItemDto
            {
                Id = id,
                Title = title,
                Description = description,
                Category = category,
                Difficulty = difficulty,
                IsDone = isDone
            };

            var resp = await _client.PutAsJsonAsync($"api/todo/{id}", dto);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
        }

        [Theory]
        [InlineData(10, "Update Task", "updated added", "Work", 5, true, 20)]
        public async void UpdateToDo_SholdReturn_BadRequest_If_DataMismatch(int id, string title, string description, string category, int difficulty, bool isDone, int wrongId)
        {
            var dto = new TodoItemDto
            {
                Id = wrongId,
                Title = title,
                Description = description,
                Category = category,
                Difficulty = difficulty,
                IsDone = isDone
            };

            var resp = await _client.PutAsJsonAsync($"api/todo/{id}", dto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateToDo_SholdReturn_BadRequest_If_NoData(int id)
        {
            var dto = new TodoItemDto();

            var resp = await _client.PutAsJsonAsync($"api/todo/{id}", dto);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Theory]
        [InlineData(10)]
        public async void DeleteToDo_SholdReturn_NoContentResponse(int id)
        {
            var resp = await _client.DeleteAsync($"api/todo/{id}");

            resp.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, resp.StatusCode);
        }

        [Theory]
        [InlineData(100)]
        public async void DeleteToDo_SholdReturn_NotFound_IfNoItem(int id)
        {
            var resp = await _client.DeleteAsync($"api/todo/{id}");


            Assert.Equal(System.Net.HttpStatusCode.NotFound, resp.StatusCode);
        }

        [Theory]
        [InlineData(1, 10, "Title", true, true, "Work", false)]
        public async void Search_WithFilter_ShouldReturn_Success(int page, int pageSize,
            string sortField, bool isAscending, bool isFilter, string category, bool isDone)
        {
            var searchRequest = new GetTodoRequest
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize,
                    IsSortAscending = isAscending,
                    SortField = sortField
                },
                IsFilter = isFilter,
                Filter = new ToDoFilter
                {
                    Category = category,
                    IsDoneFilter = isDone
                }
            };

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(1, 10, "Title", true, true, "Work", false)]
        public async void Search_WithFilter_ShouldReturn_WithCorrectPagination(int page, int pageSize,
    string sortField, bool isAscending, bool isFilter, string category, bool isDone)
        {
            var searchRequest = new GetTodoRequest
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize,
                    IsSortAscending = isAscending,
                    SortField = sortField
                },
                IsFilter = isFilter,
                Filter = new ToDoFilter
                {
                    Category = category,
                    IsDoneFilter = isDone
                }
            };

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(paginatedResponse);
            Assert.Equal(page, paginatedResponse?.Pagination?.Page);
            Assert.Equal(pageSize, paginatedResponse?.Pagination?.PageSize);
            Assert.InRange(paginatedResponse.Items.Count(), 0, pageSize);
        }

        [Theory]
        [InlineData(1, 10, "Title", true, true, "Work", false)]
        [InlineData(1, 10, "Title", true, true, "Personal", true)]
        public async void Search_WithFilter_ShouldReturn_FilteredResults(int page, int pageSize,
string sortField, bool isAscending, bool isFilter, string category, bool isDone)
        {
            var searchRequest = new GetTodoRequest
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize,
                    IsSortAscending = isAscending,
                    SortField = sortField
                },
                IsFilter = isFilter,
                Filter = new ToDoFilter
                {
                    Category = category,
                    IsDoneFilter = isDone
                }
            };

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedResponse);

            var todoList = paginatedResponse.Items.ToList();

            todoList.ForEach(item => Assert.Equal(category, item.Category));
            todoList.ForEach(item => Assert.Equal(isDone, item.IsDone));
        }

        [Theory]
        [InlineData(1, 10, "Title", true)]
        [InlineData(1, 10, "Id", true)]
        public async void Search_WithNoFilter_ShouldReturn_Results(int page, int pageSize, string sortField, bool isAscending)
        {
            var searchRequest = new GetTodoRequest
            {
                Pagination = new Pagination
                {
                    Page = page,
                    PageSize = pageSize,
                    IsSortAscending = isAscending,
                    SortField = sortField
                }
            };

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(paginatedResponse);
            var todoList = paginatedResponse.Items.ToList();
            Assert.NotEmpty(todoList);
        }

        [Fact]
        public async void Search_WithNoFilterNoPaginationNoSort_ShouldReturn_Results()
        {
            var searchRequest = new GetTodoRequest();

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            Assert.NotNull(paginatedResponse);

            var todoList = paginatedResponse.Items.ToList();

            Assert.NotEmpty(todoList);
        }

        [Theory]
        [InlineData(1000)]
        public async void Search_WithLargePageSize_ShouldReturn_LessThanMaxSize(int pageSize)
        {
            var searchRequest = new GetTodoRequest
            {
                Pagination = new Pagination
                {
                    Page = 1,
                    PageSize = pageSize
                }
            };

            var resp = await _client.PostAsJsonAsync($"api/todo/search", searchRequest);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedResponse);

            var todoList = paginatedResponse.Items.ToList();

            Assert.NotEmpty(todoList);
            Assert.Equal(60, paginatedResponse?.Pagination?.PageSize);
        }
    }
}
