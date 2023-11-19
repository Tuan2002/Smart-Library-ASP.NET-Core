using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [BindProperty]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [BindProperty]
        public string Password { get; set; } = null!;

        [BindProperty]
        public bool RememberMe { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; } = null!;
    }
}
