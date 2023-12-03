using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Unni.Admin.Application.DTOs;
using Unni.AdminAPI;

namespace Unni.Admin.FunctionalTests.Endpoints
{
    public class CategoryEndpointTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task ReturnsCategoryListWhenGetAll()
        {
            var result = await _client.GetAsync("/api/Category");

            var content = await result.Content.ReadAsStringAsync();
            var categoryList = JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotEqual(0, categoryList?.Count());
        }

        [Fact]
        public async Task ReturnsCategoryGivenId()
        {
            var result = await _client.GetFromJsonAsync<CategoryDto>("/api/Category/2");

            Assert.Equal(2, result?.Id);
            Assert.Equal("Work", result?.Name);
        }

        [Theory]
        [InlineData(150)]
        public async void GetTodoById_ShouldReturnNotFound_WhenNoItem(int id)
        {
            var result = await _client.GetAsync($"/api/Category/{id}");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Theory]
        [InlineData("New Category", "Newly added")]
        public async void AddCategory_ShouldReturnCategoryItem(string name, string description)
        {
            var dto = new AddCategoryRequest
            {
                Name = name,
                Description = description
            };

            var resp = await _client.PostAsJsonAsync<AddCategoryRequest>("api/Category", dto);

            resp.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, resp.StatusCode);
            var content = await resp.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(category);
            Assert.Equal(name, category.Name);
            Assert.Equal(4, category.Id);
        }

        [Theory]
        [InlineData(2, "Work", "updated")]
        public async void UpdateCategory_ShouldReturn_UpdatedItem(int id, string name, string description)
        {
            var dto = new CategoryDto
            {
                Id = id,
                Name = name,
                Description = description
            };

            var resp = await _client.PutAsJsonAsync($"api/Category/{id}", dto);

            resp.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

            var content = await resp.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<CategoryDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(item);
            Assert.Equal(description, item.Description);
        }
    }
}
