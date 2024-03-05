using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class TaiLieu
    {
        public int IdTaiLieu { get; set; }
        public int IdBaiGiang { get; set; }
        public string TenTaiLieu { get; set; }
        public string LoaiTaiLieu { get; set; }
        public int DuongDan { get; set; }
    }
}
