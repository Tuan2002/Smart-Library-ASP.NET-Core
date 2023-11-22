
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Areas.Admin.Models
{
    [BindProperties]
    public class CreateCategoryModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public bool Status { get; set; }
    }
}