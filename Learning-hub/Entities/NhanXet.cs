using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class NhanXet
    {
        public int MaNhanXet { get; set; }
        public string MaKhoaHoc { get; set; }
        public string TenNguoiGui { get; set; }
        public string NoiDung { get; set; }
        public int? DiemNhanXet { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
