using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Areas.Admin.Models
{
    [BindProperties]
    public class CreatePublisherModel
    {
        public string Name { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string District { get; set; } = null!;
        public string GetAddress()
        {
            return $"{District}, {Province}";
        }
    }
}