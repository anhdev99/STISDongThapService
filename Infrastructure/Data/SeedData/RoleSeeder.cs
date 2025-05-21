using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class RoleSeeder(ApplicationDbContext context) : ISeeder
{
    public void Seed()
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role
                {
                    Code = "admin",
                    Name = "Administrator",
                    DisplayName = "Quản trị viên",
                    Description = "Toàn quyền hệ thống: quản lý người dùng, cấu hình, phân quyền, dữ liệu.",
                    Order = 1,
                    Priority = true,
                    IsProtected = true,
                    Color = "bg-red-500"
                },
                new Role
                {
                    Code = "leader",
                    Name = "Leader",
                    DisplayName = "Lãnh đạo",
                    Description = "Ban giám đốc, giám sát tổng thể hoạt động và phê duyệt cấp cao.",
                    Order = 2,
                    Priority = true,
                    IsProtected = true,
                    Color = "bg-blue-500"
                },
                new Role
                {
                    Code = "manager",
                    Name = "Manager",
                    DisplayName = "Trưởng phòng",
                    Description = "Quản lý công việc và nhân sự trong phòng ban, phân công và phê duyệt nội bộ.",
                    Order = 3,
                    Priority = false,
                    IsProtected = true,
                    Color = "bg-green-500"
                },
                new Role
                {
                    Code = "user",
                    Name = "User",
                    DisplayName = "Người dùng",
                    Description = "Nhân viên thông thường, thực hiện công việc được giao và báo cáo.",
                    Order = 4,
                    Priority = false,
                    IsProtected = true,
                    Color = "bg-yellow-500"
                }
            );
            context.SaveChanges();
        }
    }
}
