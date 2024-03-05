using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class CauHoi
    {
        public int MaCauHoi { get; set; }
        public string MaKhoaHoc { get; set; }
        public string CauHoi1 { get; set; }
        public string DapAn1 { get; set; }
        public string DapAn2 { get; set; }
        public string DapAn3 { get; set; }
        public string DapAn4 { get; set; }
        public string DapAnDung { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
