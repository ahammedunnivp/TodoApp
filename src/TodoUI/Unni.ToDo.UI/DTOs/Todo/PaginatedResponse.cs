namespace Unni.ToDo.UI.DTOs.Todo
{
    public class PaginatedResponse<T>
    {
        public Pagination? Pagination { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public PaginatedResponse()
        {
        }
    }

    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortField { get; set; }
        public bool IsSortAscending { get; set; } = true;
        public int? TotalCount { get; set; }
    }

}
