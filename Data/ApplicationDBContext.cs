using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Library.Entities;

namespace Smart_Library.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, UserRole, Guid>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            ApplicationUser User = new ApplicationUser
            {
                Id = userId, // primary key
                UserName = "admin@admin.com",
                NormalizedUserName = "admin@admin.com".ToUpper(),
                Email = "admin@admin.com",
                NormalizedEmail = "admin@admin.com".ToUpper(),
                EmailConfirmed = true,
                FirstName = "Nguyễn Ngọc Anh",
                LastName = "Tuấn",
                ProfileImage = "https://i.imgur.com/6NQ1n0V.png",
                CreatedAt = DateTime.Now,
                DateOfBirth = new DateOnly(2002, 07, 02),
                PhoneNumber = "0123456789"
            };
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            User.PasswordHash = passwordHasher.HashPassword(User, "Admin@123");
            modelBuilder.Entity<ApplicationUser>().HasData(User);
            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = roleId, Name = "Admin", RoleDescription = "Quản trị viên", NormalizedName = "ADMIN".ToUpper() });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = roleId,
                    UserId = userId
                }
            );
        }
    }
}