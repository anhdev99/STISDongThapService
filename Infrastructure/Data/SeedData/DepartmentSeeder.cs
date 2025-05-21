using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class DepartmentSeeder(ApplicationDbContext context) : ISeeder
{
    public void Seed()
    {
        if (!context.Departments.Any())
        {
            context.Departments.AddRange(
                new Department()
                {
                    Id = 1,
                    Code = "D000001",
                    Name = "Sở Khoa học và Công nghệ",
                    Order = 10
                },
                new Department()
                {
                    Id = 2,
                    Code = "D000002",
                    Name = "Lãnh đạo sở",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 3,
                    Code = "D000003",
                    Name = "Văn phòng sở",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 4,
                    Code = "D000004",
                    Name = "Thanh tra sở",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 5,
                    Code = "D000005",
                    Name = "Phòng Khoa học và Công nghệ",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 6,
                    Code = "D000006",
                    Name = "Phòng Tiêu chuẩn - Đo lường - Chất lượng",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 7,
                    Code = "D000007",
                    Name = " Phòng Chuyển đổi số và bưu chính viễn thông",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 8,
                    Code = "D000008",
                    Name = "Trung tâm Kiểm định và Kiểm nghiệm Đồng Tháp",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 9,
                    Code = "D000009",
                    Name = "Phòng Hành chính - Tổng hợp",
                    Order = 9,
                    ParentId = 8
                },
                new Department()
                {
                    Id = 10,
                    Code = "D000010",
                    Name = "Phòng Phân tích thử nghiệm",
                    Order = 9,
                    ParentId = 8
                },
                new Department()
                {
                    Id = 11,
                    Code = "D000011",
                    Name = " Phòng Thông tin và Ứng dụng Khoa học công nghệ",
                    Order = 9,
                    ParentId = 8
                },
                new Department()
                {
                    Id = 12,
                    Code = "D000012",
                    Name = "Phòng Tư vấn và Dịch vụ kỹ thuật",
                    Order = 9,
                    ParentId = 8
                },
                new Department()
                {
                    Id = 13,
                    Code = "D000013",
                    Name = "Phòng Kiểm định - Hiệu chuẩn",
                    Order = 9,
                    ParentId = 8
                },
                new Department()
                {
                    Id = 14,
                    Code = "D000014",
                    Name = "Trung tâm Chuyển đổi số",
                    Order = 9,
                    ParentId = 1
                },
                new Department()
                {
                    Id = 15,
                    Code = "D000015",
                    Name = "Phòng Điều hành thông minh - Tổng hợp",
                    Order = 9,
                    ParentId = 14
                },
                new Department()
                {
                    Id = 16,
                    Code = "D000016",
                    Name = "Phòng Hạ tầng - An toàn thông tin",
                    Order = 9,
                    ParentId = 14
                },
                new Department()
                {
                    Id = 17,
                    Code = "D000017",
                    Name = "Phòng Chính quyền số - Dịch vụ công trực tuyến",
                    Order = 9,
                    ParentId = 14
                },
                new Department()
                {
                    Id = 18,
                    Code = "D000018",
                    Name = "Phòng Phát triển phần mềm",
                    Order = 9,
                    ParentId = 14
                },
                new Department()
                {
                    Id = 19,
                    Code = "D000019",
                    Name = "Phòng An toàn thông tin",
                    Order = 9,
                    ParentId = 14
                },
                new Department()
                {
                    Id = 20,
                    Code = "D000020",
                    Name = "Trung tâm Ứng dụng nông nghiệp công nghệ cao",
                    Order = 9,
                    ParentId = 1
                }
            );
            context.SaveChanges();
        }
    }
}