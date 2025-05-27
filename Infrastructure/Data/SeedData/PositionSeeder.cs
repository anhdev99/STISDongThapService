
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class PositionSeeder(ApplicationDbContext context) : ISeeder
{
    public  void Seed()
    {
        var positions = new List<Position>
        {
            new Position { Name = "Giám đốc sở", Code = "giam_doc", Order = 1 },
            new Position { Name = "Phó Giám đốc sở", Code = "pho_giam_doc", Order = 2 },
            new Position { Name = "Chánh Văn phòng", Code = "chanh_van_phong", Order = 3 },
            new Position { Name = "Phó Chánh Văn phòng", Code = "pho_chanh_van_phong", Order = 4 },
            new Position { Name = "Kế toán trưởng", Code = "ke_toan_truong", Order = 5 },
            new Position { Name = "Kế toán", Code = "ke_toan", Order = 6 },
            new Position { Name = "Kế toán viên", Code = "ke_toan_vien", Order = 7 },
            new Position { Name = "Văn thư", Code = "van_thu", Order = 8 },
            new Position { Name = "Chuyên viên", Code = "chuyen_vien", Order = 9 },
            new Position { Name = "Nhân viên", Code = "nhan_vien", Order = 10 },
            new Position { Name = "Chánh Thanh tra", Code = "chanh_thanh_tra", Order = 11 },
            new Position { Name = "Phó Chánh Thanh tra", Code = "pho_chanh_thanh_tra", Order = 12 },
            new Position { Name = "Thanh tra viên chính", Code = "thanh_tra_vien_chinh", Order = 13 },
            new Position { Name = "Trưởng phòng", Code = "truong_phong", Order = 14 },
            new Position { Name = "Phó Trưởng phòng", Code = "pho_truong_phong", Order = 15 },
            new Position { Name = "Viên chức", Code = "vien_chuc", Order = 16 },
            new Position { Name = "Tài xế", Code = "tai_xe", Order = 17 },
            new Position { Name = "Tạp vụ", Code = "tap_vu", Order = 18 },
            new Position { Name = "Bảo vệ", Code = "bao_ve", Order = 19 },
            new Position { Name = "Văn Thư – Thủ quỹ", Code = "van_thu_thu_quy", Order = 20 },
            new Position { Name = "Giám Đốc", Code = "giam_doc_2", Order = 21 },
            new Position { Name = "Phó Giám Đốc", Code = "pho_giam_doc_2", Order = 21 },
            new Position { Name = "Q. Trưởng phòng", Code = "quyen_truong_phong", Order = 22 },
            new Position { Name = "P. Trưởng phòng - PTĐH", Code = "pho_truong_phong_ptdh", Order = 23 }
        };

        foreach (var position in positions)
        {
            if (!context.Positions.Any(x => x.Code == position.Code))
            {
                context.Positions.Add(position);
            }
        }

        context.SaveChanges();
    }
}