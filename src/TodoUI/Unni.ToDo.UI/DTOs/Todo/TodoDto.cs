namespace Unni.ToDo.UI.DTOs.Todo
{
    public class TodoDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; } = false;
        public int? Difficulty { get; set; }
        public string? Category { get; set; }
    }
}
