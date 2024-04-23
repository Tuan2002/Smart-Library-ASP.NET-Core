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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Workspace ExampleWorkspace = new Workspace
            // {
            //     WorkspaceId = 1,
            //     WorkspaceName = "Viện KT và CN",
            //     CreatedAt = DateTime.Now
            // };
            // ApplicationUser ExampleUser = new ApplicationUser
            // {
            //     UserName = "admin@admin.com",
            //     NormalizedUserName = "admin@admin.com".ToUpper(),
            //     Email = "admin@admin.com",
            //     NormalizedEmail = "admin@admin.com".ToUpper(),
            //     EmailConfirmed = true,
            //     FirstName = "Nguyễn Ngọc Anh",
            //     LastName = "Tuấn",
            //     ProfileImage = "/uploads/images/admin.webp",
            //     Address = "Vinh, Nghệ An",
            //     WorkspaceId = ExampleWorkspace.WorkspaceId,
            //     CreatedAt = DateTime.Now,
            //     DateOfBirth = new DateOnly(2002, 07, 02),
            //     PhoneNumber = "0123456789"
            // };
            // PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            // ExampleUser.PasswordHash = passwordHasher.HashPassword(ExampleUser, "Admin@123");

            // IdentityRole ExampleRole = new IdentityRole
            // {
            //     Name = "Quản trị viên",
            //     NormalizedName = "QUẢN TRỊ VIÊN"
            // };
            // IdentityUserRole<string> UserRole = new IdentityUserRole<string>
            // {
            //     RoleId = ExampleRole.Id,
            //     UserId = ExampleUser.Id
            // };
            List<OrderStatus> orderStatuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                    StatusId = 1,
                    StatusName = "Đang chờ xử lý",
                    Description = "Đơn mượn sách đang chờ xử lý"
                },
                new OrderStatus
                {
                    StatusId = 2,
                    StatusName = "Chờ lấy sách",
                    Description = "Đơn đang chờ lấy sách"
                },
                new OrderStatus
                {
                    StatusId = 3,
                    StatusName = "Đã bàn giao sách",
                    Description = "Đã bàn giáo sách cho người mượn"
                },
                new OrderStatus
                {
                    StatusId = 4,
                    StatusName = "Đã trả sách",
                    Description = "Đã trả sách cho thủ thư"
                },
                new OrderStatus
                {
                    StatusId = 5,
                    StatusName = "Quá hạn trả sách",
                    Description = "Đơn mượn sách quá hạn trả sách"
                },
                new OrderStatus
                {
                    StatusId = 6,
                    StatusName = "Đã hủy",
                    Description = "Đơn mượn sách đã hủy"
                }

            };
            // modelBuilder.Entity<Workspace>().HasData(ExampleWorkspace);
            // modelBuilder.Entity<ApplicationUser>().HasData(ExampleUser);
            // modelBuilder.Entity<IdentityRole>().HasData(ExampleRole);
            // modelBuilder.Entity<IdentityUserRole<string>>().HasData(UserRole);
            modelBuilder.Entity<OrderStatus>().HasData(orderStatuses);
        }
        public DbSet<Workspace> Workspace { get; set; } = null!;
        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Author> Author { get; set; } = null!;
        public DbSet<Publisher> Publisher { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatus { get; set; } = null!;
    }
}