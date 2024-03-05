using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class DangKyHoc
    {
        public DangKyHoc()
        {
            ThanhToans = new HashSet<ThanhToan>();
        }

        public int MaDangKy { get; set; }
        public string MaKhoaHoc { get; set; }
        public int MaHocVien { get; set; }
        public string TieuDeKhoaHoc { get; set; }
        public decimal? HocPhi { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayHuy { get; set; }
        public int? NguoiHuy { get; set; }
        public string TinhTrang { get; set; }

        public virtual HocVien MaHocVienNavigation { get; set; }
        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
