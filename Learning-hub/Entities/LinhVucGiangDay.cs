using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class LinhVucGiangDay
    {
        public int MaLinhVuc { get; set; }
        public int? MaNguoiDay { get; set; }
        public int? MaDanhmuc { get; set; }

        public virtual DanhMucKhoaHoc MaDanhmucNavigation { get; set; }
        public virtual NguoiDay MaNguoiDayNavigation { get; set; }
    }
}
