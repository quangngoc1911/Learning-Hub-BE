using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class DanhMucCon
    {
        public int MaDanhMucCon { get; set; }
        public int? MaDanhMuc { get; set; }
        public string TieuDeDanhMuc { get; set; }

        public virtual DanhMucKhoaHoc MaDanhMucNavigation { get; set; }
    }
}
