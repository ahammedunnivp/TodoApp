namespace Unni.ToDo.UI.DTOs.Todo
{
    public class CreateTodoRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Difficulty { get; set; }
        public string? Category { get; set; }
    }
}
