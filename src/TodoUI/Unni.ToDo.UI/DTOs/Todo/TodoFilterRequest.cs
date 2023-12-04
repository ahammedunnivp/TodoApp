namespace Unni.ToDo.UI.DTOs.Todo
{
    public class TodoFilterRequest
    {
        public Pagination? Pagination { get; set; }
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
