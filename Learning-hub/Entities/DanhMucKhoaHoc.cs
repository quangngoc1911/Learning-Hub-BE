using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class DanhMucKhoaHoc
    {
        public DanhMucKhoaHoc()
        {
            DanhMucCons = new HashSet<DanhMucCon>();
            LinhVucGiangDays = new HashSet<LinhVucGiangDay>();
        }

        public int MaDanhMuc { get; set; }
        public string TieuDeDanhMuc { get; set; }

        public virtual ICollection<DanhMucCon> DanhMucCons { get; set; }
        public virtual ICollection<LinhVucGiangDay> LinhVucGiangDays { get; set; }
    }
}
