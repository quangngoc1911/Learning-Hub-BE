using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class HoiDap
    {
        public HoiDap()
        {
            TraLoiHoiDaps = new HashSet<TraLoiHoiDap>();
        }

        public int MaHoiDap { get; set; }
        public string MaKhoaHoc { get; set; }
        public int? MaHocVien { get; set; }
        public string TenNguoiGui { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual HocVien MaHocVienNavigation { get; set; }
        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
        public virtual ICollection<TraLoiHoiDap> TraLoiHoiDaps { get; set; }
    }
}
