using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Entities;

namespace Smart_Library.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            ApplicationUser ExampleUser = new ApplicationUser
            {
                UserName = "admin@admin.com",
                NormalizedUserName = "admin@admin.com".ToUpper(),
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                EmailConfirmed = true,
                FirstName = "Nguyễn Ngọc Anh",
                LastName = "Tuấn",
                ProfileImage = "/upload/user-upload/admin.webp",
                CreatedAt = DateTime.Now,
                DateOfBirth = new DateOnly(2002, 07, 02),
                PhoneNumber = "0123456789"
            };
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            ExampleUser.PasswordHash = passwordHasher.HashPassword(ExampleUser, "Admin@123");

            IdentityRole ExampleRole = new IdentityRole
            {
                Name = "Quản trị viên",
                NormalizedName = "QUẢN TRỊ VIÊN"
            };
            IdentityUserRole<string> UserRole = new IdentityUserRole<string>
            {
                RoleId = ExampleRole.Id,
                UserId = ExampleUser.Id
            };
            modelBuilder.Entity<ApplicationUser>().HasData(ExampleUser);
            modelBuilder.Entity<IdentityRole>().HasData(ExampleRole);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(UserRole);
        }
    }
}