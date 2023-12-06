namespace Unni.Todo.Application.DTOs
{
    public class GetTodoRequest : BaseDataRequest
    {
        public bool IsFilter { get; set; } = false;
        public ToDoFilter? Filter { get; set; }

    }

    public class ToDoFilter
    {
        public bool? IsDoneFilter { get; set; }
        public int? Difficulty { get; set; }
        public string? Category { get; set; }
    }
}
