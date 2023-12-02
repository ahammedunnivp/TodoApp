namespace Unni.ToDo.Core.DTOs
{
    public class PaginatedResponseDto<T>
    {
        public Pagination? Pagination { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public PaginatedResponseDto(Pagination pagination, IEnumerable<T> items, int total_count)
        {
            Items = items;
            Pagination = new Pagination
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                SortField = pagination.SortField,
                IsSortAscending = pagination.IsSortAscending,
                TotalCount = total_count
            };
        }

        public PaginatedResponseDto()
        {
        }

        public PaginatedResponseDto(int page, int pageSize, IEnumerable<T> items, int total_count)
        {
            Items = items;
            Pagination = new Pagination
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total_count
            };
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
