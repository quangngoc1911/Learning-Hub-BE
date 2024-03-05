using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class Chuong
    {
        public Chuong()
        {
            BaiGiangs = new HashSet<BaiGiang>();
        }

        public int MaChuong { get; set; }
        public string MaKhoaHoc { get; set; }
        public string TieuDeChuong { get; set; }
        public string TinhTrang { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
        public virtual ICollection<BaiGiang> BaiGiangs { get; set; }
    }
}
