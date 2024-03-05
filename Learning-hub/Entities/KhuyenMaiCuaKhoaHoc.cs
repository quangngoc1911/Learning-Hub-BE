using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class KhuyenMaiCuaKhoaHoc
    {
        public int MaApDung { get; set; }
        public string MaKhoaHoc { get; set; }
        public int? PhanTramGiamGia { get; set; }
        public int? GiaApDungMa { get; set; }
        public int? GiaKhiGiam { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
