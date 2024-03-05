using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class KhuyenMai
    {
        public int MaGiamGia { get; set; }
        public int? MaNguoiDay { get; set; }
        public double? GiamGia { get; set; }
        public string GhiChu { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual NguoiDay MaNguoiDayNavigation { get; set; }
    }
}
