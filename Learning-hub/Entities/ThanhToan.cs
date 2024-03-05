using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int? MaNguoiDay { get; set; }
        public int? MaDangKy { get; set; }
        public decimal? SotTien { get; set; }
        public decimal? SoTienNhanDuoc { get; set; }
        public string TinhTrang { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        public virtual DangKyHoc MaDangKyNavigation { get; set; }
        public virtual NguoiDay MaNguoiDayNavigation { get; set; }
    }
}
