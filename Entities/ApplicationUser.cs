using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Smart_Library.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        [Required]
        public string LastName { get; set; } = null!;
        public string? ProfileImage { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string? Address { get; set; }
        [ForeignKey("WorkspaceId")]
        public int WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}