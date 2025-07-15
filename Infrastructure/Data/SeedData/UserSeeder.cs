using Core.Entities;
using Core.Interfaces;
using Shared.Helpers;

namespace Infrastructure.Data.SeedData;

public class UserSeeder(ApplicationDbContext context) : ISeeder
{
    public void Seed()
    {
        if (!context.User.Any())
        {
            byte[] passwordHash, passwordSalt;
            PasswordHelper.GeneratePasswordHash("123456", out passwordHash, out passwordSalt);

            var users = new List<User>
            {
                // Văn phòng Sở - DepartmentId = 2
                CreateUser("Nguyễn Quốc Cường", "nqcuong", 2, 3, passwordHash, passwordSalt),
                CreateUser("Lê Hồng Châu", "lhchau", 2, 4, passwordHash, passwordSalt),
                CreateUser("Nguyễn Thị Thanh Thủy", "nttthuy", 2, 4, passwordHash, passwordSalt),
                CreateUser("Lê Thị Liên Phương", "ltlphuong", 2, 4, passwordHash, passwordSalt),
                CreateUser("Nguyễn Thị Thanh Tâm", "ntttam", 2, 4, passwordHash, passwordSalt),
                CreateUser("Ngô Bá Khởi", "nbkhoi", 2, 4, passwordHash, passwordSalt),
                CreateUser("Nguyễn Thanh Thảo", "ntthao", 2, 4, passwordHash, passwordSalt),
                CreateUser("Trần Thanh Cường", "ttcuong", 2, 4, passwordHash, passwordSalt),

                // Phòng Khoa học - DepartmentId = 3
                CreateUser("Phan Hà", "pha", 3, 14, passwordHash, passwordSalt),
                CreateUser("Nguyễn Minh Nhựt Quang", "nmnquang", 3, 15, passwordHash, passwordSalt),
                CreateUser("Trương Võ Phú Tân", "tvptan", 3, 15, passwordHash, passwordSalt),

                // Phòng Công nghệ và Đổi mới sáng tạo - DepartmentId = 4
                CreateUser("Phan Vinh Quang", "pvquang", 4, 14, passwordHash, passwordSalt),
                CreateUser("Nguyễn Minh Thư", "nmthu", 4, 15, passwordHash, passwordSalt),
                CreateUser("Bùi Thị Liên", "btlien", 4, 15, passwordHash, passwordSalt),

                // Phòng Chuyển đổi số - DepartmentId = 5
                CreateUser("Hồ Nguyễn Công Trình", "hnctrinh", 5, 14, passwordHash, passwordSalt),
                CreateUser("Đỗ Đức Thông", "ddthong", 5, 15, passwordHash, passwordSalt),
                CreateUser("Đinh Thanh Trung", "dtttrung", 5, 15, passwordHash, passwordSalt),
                CreateUser("Võ Văn Sang", "vvsang", 5, 15, passwordHash, passwordSalt),

                // Phòng Tiêu chuẩn Đo lường Chất lượng - DepartmentId = 6
                CreateUser("Nguyễn Thành Long", "ntlong", 6, 14, passwordHash, passwordSalt),
                CreateUser("Lê Minh Đúng", "lmdung", 6, 15, passwordHash, passwordSalt),
                CreateUser("Lê Thị Thanh Trúc", "ltttruc", 6, 15, passwordHash, passwordSalt),

                // Trung tâm Chuyển đổi số - DepartmentId = 7
                CreateUser("Lê Thị Kim Loan", "ltkloan", 7, 1, passwordHash, passwordSalt),
                CreateUser("Nguyễn Minh Sang", "nmsang", 7, 2, passwordHash, passwordSalt),
                CreateUser("Trần Văn Triều", "tvtrieu", 7, 2, passwordHash, passwordSalt),
                CreateUser("Nguyễn Phúc Tâm", "nptam", 7, 2, passwordHash, passwordSalt),
                CreateUser("Trần Phước Dư", "tpdu", 7, 2, passwordHash, passwordSalt),

                // Trung tâm Nghiên cứu Ứng dụng và Dịch vụ khoa học công nghệ - DepartmentId = 8
                CreateUser("Đặng Thị Hồng Yến", "dthyen", 8, 1, passwordHash, passwordSalt),
                CreateUser("Lê Văn Thoại", "lvthoai", 8, 2, passwordHash, passwordSalt),
                CreateUser("Huỳnh Trung Tín", "httin", 8, 2, passwordHash, passwordSalt),
                CreateUser("Trần Văn Nhãn", "tvnhan", 8, 2, passwordHash, passwordSalt),
                CreateUser("Nguyễn Bích Phượng", "nbphuong", 8, 2, passwordHash, passwordSalt),
                CreateUser("Trần Thị Thưa", "ttthua", 8, 2, passwordHash, passwordSalt),
                CreateUser("Võ Trung Hiếu", "vthieu", 8, 2, passwordHash, passwordSalt)
            };

            context.User.AddRange(users);
            context.SaveChanges();
        }
    }

    private User CreateUser(string fullName, string userName, int departmentId, int positionId, byte[] hash, byte[] salt)
    {
        var names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lastName = names[0];
        var firstName = string.Join(' ', names.Skip(1));

        return new User
        {
            UserName = userName.ToLower(),
            PasswordHash = hash,
            PasswordSalt = salt,
            LastName = lastName,
            FirstName = firstName,
            DepartmentId = departmentId,
            PositionId = positionId,
            IsVerified = true,
            UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    RoleId = 4
                }
            }
        };
    }
}
