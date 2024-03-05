using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class BaoCao
    {
        public int MaBaoCao { get; set; }
        public string MaKhoaHoc { get; set; }
        public int? MaHocVien { get; set; }
        public string ChiTietBaoCao { get; set; }
        public string TieuDe { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public string TinhTrang { get; set; }
        public string PhanHoi { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual HocVien MaHocVienNavigation { get; set; }
        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
