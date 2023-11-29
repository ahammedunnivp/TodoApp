﻿using Unni.ToDo.API.Enums;

namespace Unni.ToDo.API.Data.Models
{
    public class TodoItemEntity
    {
        public int? Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public int? Difficulty { get; set; }
        public string? Category { get; set; }
    }
}
