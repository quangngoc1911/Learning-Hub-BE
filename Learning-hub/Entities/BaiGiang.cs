using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class BaiGiang
    {
        public int MaBaiGiang { get; set; }
        public int? MaChuong { get; set; }
        public string TieuDeBaiGiang { get; set; }
        public int? ThoiLuong { get; set; }
        public string Video { get; set; }
        public bool? XemTruocVideo { get; set; }
        public string FileTaiLieu { get; set; }
        public string TinhTrang { get; set; }

        public virtual Chuong MaChuongNavigation { get; set; }
    }
}
