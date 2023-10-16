using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Smart_Library.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        [Required]
        public string LastName { get; set; } = null!;
        public string? ProfileImage { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}