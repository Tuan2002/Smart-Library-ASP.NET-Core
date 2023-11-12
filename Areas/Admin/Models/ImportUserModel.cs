using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart_Library.Admin.Models;

namespace Smart_Library.Admin.Models
{
    [BindProperties]
    public class ImportUserModel
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string GetAddress()
        {
            return District + ", " + Province;
        }
        public string GeneratePassword()
        {
            string? defaultUsername = Email?.Split("@")[0];
            string? defaultPassword = defaultUsername + "@" + DateOfBirth.ToString("yyyy");
            return defaultPassword;
        }
    }
}