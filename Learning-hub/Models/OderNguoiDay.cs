using System;

namespace Learning_hub.Models
{
    public class OderNguoiDay
    {
        public class DonHang
        {
            public int MaNguoiDay { get; set; }
            public int MaDangKy { get; set; }
            public string MaKhoaHoc { get; set; }
            public int MaHocVien { get; set; }
            public string TieuDeKhoaHoc { get; set; }
            public decimal HocPhi { get; set; }
            public DateTime NgayThanhToan { get; set; }
            public string TinhTrang { get; set; }
        }
    }
}
