using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class DepartmentSeeder(ApplicationDbContext context) : ISeeder
{
    public void Seed()
    {
        if (!context.Departments.Any())
        {
            var parent = new Department()
            {
                Code = "D000001",
                Name = "Sở Khoa học và Công nghệ",
                Order = 10
            };

            var departments = new List<Department>
            {
                parent,
                new Department()
                {
                    Code = "D000002",
                    Name = "Văn phòng sở",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000003",
                    Name = "Phòng Khoa học",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000004",
                    Name = "Phòng Công nghệ và Đổi mới sáng tạo",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000005",
                    Name = "Phòng Chuyển đổi số",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000006",
                    Name = "Phòng Tiêu chuẩn Đo lường Chất lượng",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000007",
                    Name = "Trung tâm Chuyển đổi số",
                    Order = 9,
                    Parent = parent
                },
                new Department()
                {
                    Code = "D000008",
                    Name = "Trung tâm Nghiên cứu Ứng dụng và Dịch vụ khoa học công nghệ",
                    Order = 9,
                    Parent = parent
                }
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();
        }
    }
}