using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class NguoiDay
    {
        public NguoiDay()
        {
            KhoaHocs = new HashSet<KhoaHoc>();
            KhuyenMais = new HashSet<KhuyenMai>();
            LinhVucGiangDays = new HashSet<LinhVucGiangDay>();
            ThanhToans = new HashSet<ThanhToan>();
            TraLoiHoiDaps = new HashSet<TraLoiHoiDap>();
        }

        public int MaNguoiDay { get; set; }
        public string TenNguoiDay { get; set; }
        public string Email { get; set; }
        public string GioiThieuBanThan { get; set; }
        public string HinhAnh { get; set; }
        public string TinhTrang { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Cccd { get; set; }
        public DateTime? NgayTao { get; set; }
        public string Role { get; set; }

        public virtual ICollection<KhoaHoc> KhoaHocs { get; set; }
        public virtual ICollection<KhuyenMai> KhuyenMais { get; set; }
        public virtual ICollection<LinhVucGiangDay> LinhVucGiangDays { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
        public virtual ICollection<TraLoiHoiDap> TraLoiHoiDaps { get; set; }
    }
}
