using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class TraLoiHoiDap
    {
        public int MaTraLoi { get; set; }
        public int? MaNguoiDay { get; set; }
        public int? MaHoiDap { get; set; }
        public string TenNguoiTraLoi { get; set; }
        public string MoTa { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual HoiDap MaHoiDapNavigation { get; set; }
        public virtual NguoiDay MaNguoiDayNavigation { get; set; }
    }
}
