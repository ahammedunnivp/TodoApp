using Microsoft.Extensions.Logging;
using Moq;
using Unni.ToDo.Core.Models;
using Unni.ToDo.Infrastructure.Data.Repositories;

namespace Unni.ToDo.Tests.RepositoryTests
{
    public class ToDoRepositoryTests
    {
        private readonly TodoItemRepository _repo;
        private readonly ToDoDBContext _dbContext;
        private readonly Mock<ILogger<TodoItemRepository>> _logger =
            new Mock<ILogger<TodoItemRepository>>();

        public ToDoRepositoryTests()
        {
            _dbContext = ContextGenerator.Generate();
            _repo = new TodoItemRepository(_dbContext, _logger.Object);
        }

        [Fact]
        public void AddTodoItem_Returns_Item_With_Id()
        {
            var entity = this.GetValidEntity();

            var resp = _repo.Add(entity);

            Assert.IsType<TodoItemEntity>(resp);
            Assert.IsType<int>(resp.Id);

        }

        [Fact]
        public void GetById_Returns_AddedItem()
        {
            var entity = this.GetValidEntity();
            var resp = _repo.Add(entity);


            Assert.IsType<int>(resp.Id);
            var item = _repo.GetById(resp.Id.Value);

            Assert.IsType<TodoItemEntity>(item);
            Assert.Equal(item.Id, resp.Id);
        }

        [Theory]
        [InlineData("Updated Title")]
        public void Update_Returns_UpdatedItem(string updatedTitle)
        {
            var entity = this.GetValidEntity();
            var resp = _repo.Add(entity);

            resp.Title = updatedTitle;

            var updateResponse = _repo.Update(resp);
            var updatedItem = _repo.GetById(resp.Id.Value);

            Assert.IsType<TodoItemEntity>(updateResponse);
            Assert.IsType<TodoItemEntity>(updatedItem);
            Assert.Equal(updatedItem.Title, updatedTitle);
        }

        [Fact]
        public void Delete_Item_IsRemovedFromDB()
        {
            var entity = this.GetValidEntity();
            var item = _repo.Add(entity);

            _repo.Delete(item.Id.Value);

            var resp = _repo.GetById(item.Id.Value);

            Assert.Null(resp);
        }

        private TodoItemEntity GetValidEntity(int? id = null, string? title = "Sample",
            string? category = "Work")
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

        //[Theory]
        //[InlineData(1, 3)]
        //public void SearchTodoItem_Returns_FilteredResult_SortByTitle(int page, int pageSize)
        //{
        //    List<TodoItemEntity> items = new List<TodoItemEntity>();
        //    for(int i=0; i<10; i++)
        //    {
        //        var item = this.GetValidEntity(title: $"Task {i+1}");
        //        item.Difficulty = i+1/2;
        //        item.Category = i % 2 == 0 ? "Work" : "Personal";
        //        items.Add(item);
        //    }

        //    var pagination = new Pagination
        //    {
        //        Page = page,
        //        PageSize = pageSize,
        //        SortField = "Difficulty",
        //        IsSortAscending = false
        //    };

        //    var filter = new ToDoFilter
        //    {
        //        IsDoneFilter = false,
        //        Category = "Work"
        //    };


        //    items.ForEach(item => _repo.Add(item));

        //    (var resp, var total_count) = _repo.Search(pagination, null);

        //    var itemsList = resp.ToList();

        //    Assert.Equal(pageSize, itemsList.Count());
        //    Assert.Equal("Task 10", itemsList[0].Title);
        //}

    }


}
