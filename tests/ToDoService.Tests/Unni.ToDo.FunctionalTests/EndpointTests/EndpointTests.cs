using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using Unni.Todo.WebAPI;
using Unni.ToDo.Core.DTOs;
using Unni.ToDo.Core.Models;
using Unni.ToDo.Infrastructure.Data.Repositories;

namespace Unni.ToDo.Tests.EndpointTests
{
    public class EndpointTests
    {
        private readonly HttpClient _client;


        public EndpointTests()
        {
            var server = new TestServer(new WebHostBuilder().ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ToDoDBContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ToDoDBContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDBContext>();
                    dbContext.Database.EnsureCreated();
                    SeedData(dbContext);
                }
            })
            .UseStartup<Startup>());

            _client = server.CreateClient();
        }

        [Theory]
        [InlineData(1)]
        public async void GetTodoById_ShouldReturnTodoItem(int id)
        {
            var resp = await _client.GetAsync($"api/todo/{id}");
            var content = await resp.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<TodoItemDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(id, todo?.Id.Value);
            resp.EnsureSuccessStatusCode();
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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("api/todo", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("api/todo", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("api/todo", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("api/todo", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync("api/todo", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PutAsync($"api/todo/{id}", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PutAsync($"api/todo/{id}", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PutAsync($"api/todo/{id}", jsonContent);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, resp.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateToDo_SholdReturn_BadRequest_If_NoData(int id)
        {
            var dto = new TodoItemDto();
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var resp = await _client.PutAsync($"api/todo/{id}", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

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
            var jsonContent = new StringContent(JsonSerializer.Serialize(searchRequest), Encoding.UTF8, "application/json");

            var resp = await _client.PostAsync($"api/todo/search", jsonContent);

            resp.EnsureSuccessStatusCode();

            var content = await resp.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponseDto<TodoItemDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedResponse);

            var todoList = paginatedResponse.Items.ToList();

            Assert.NotEmpty(todoList);
            Assert.Equal(60, paginatedResponse?.Pagination?.PageSize);
        }

        private void SeedData(ToDoDBContext dBContext)
        {
            if (!dBContext.ToDoItems.Any())
            {
                dBContext.ToDoItems.AddRange(
                    new TodoItemEntity { Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = true },
                    new TodoItemEntity { Title = "Task 2", Category = "Personal", Difficulty = 2, IsDone = true },
                    new TodoItemEntity { Title = "Task 3", Category = "Work", Difficulty = 3, IsDone = true },
                    new TodoItemEntity { Title = "Task 4", Category = "Personal", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Title = "Task 5", Category = "Work", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Title = "Task 6", Category = "Personal", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Title = "Task 7", Category = "Work", Difficulty = 2, IsDone = false },
                    new TodoItemEntity { Title = "Task 8", Category = "Personal", Difficulty = 3, IsDone = false },
                    new TodoItemEntity { Title = "Task 9", Category = "Work", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Title = "Task 10", Category = "Personal", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = false }
                    );
                dBContext.SaveChanges();
            }
        }
    }
}
