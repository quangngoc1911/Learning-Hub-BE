using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class TienTrinhHoc
    {
        public int MaTienTrinh { get; set; }
        public int? MaHocVien { get; set; }
        public string MaKhoaHoc { get; set; }
        public string TieuDeChuong { get; set; }
        public string TieuDeBaiGiang { get; set; }
        public int? TongBaiGiang { get; set; }
        public int? BaiGiangHocDuoc { get; set; }

        public virtual HocVien MaHocVienNavigation { get; set; }
        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
