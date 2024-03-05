using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class HocVien
    {
        public HocVien()
        {
            BaoCaos = new HashSet<BaoCao>();
            DangKyHocs = new HashSet<DangKyHoc>();
            HoiDaps = new HashSet<HoiDap>();
            TienTrinhHocs = new HashSet<TienTrinhHoc>();
        }

        public int MaHocVien { get; set; }
        public string TenHocVien { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Email { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public DateTime? NgayTao { get; set; }
        public string Role { get; set; }

        public virtual ICollection<BaoCao> BaoCaos { get; set; }
        public virtual ICollection<DangKyHoc> DangKyHocs { get; set; }
        public virtual ICollection<HoiDap> HoiDaps { get; set; }
        public virtual ICollection<TienTrinhHoc> TienTrinhHocs { get; set; }
    }
}
