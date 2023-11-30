﻿using System.ComponentModel.DataAnnotations;

namespace Unni.ToDo.Common.Models
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}
