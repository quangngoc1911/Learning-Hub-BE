using Microsoft.AspNetCore.Http;

namespace Learning_hub.Models
{
    public class TaoKhoaHocModel
    {
        public class GioiThieuKhoaHoc
        {
            public string MaKhoaHoc { get; set; }
            public int MaNguoiDay { get; set; }
            public string TieuDeKhoaHoc { get; set; }
            public string MoTa { get; set; }
            public string KienThucThuDuoc { get; set; }
            public string TrinhDo { get; set; }
            public string DanhMuc { get; set; }
            public string DanhMucCon { get; set; }
            public IFormFile HinhAnh { get; set; }
            public string TinhTrang { get; set; }
        }
        public class ChuongTrinhGiangDay
        {
            public string TieuDeKhoaHoc { get; set; }
            public string MoTa { get; set; }
            public string KienThucThuDuoc { get; set; }
            public string TrinhDo { get; set; }
            public string DanhMuc { get; set; }
            public string DanhMucCon { get; set; }
            public IFormFile HinhAnh { get; set; }
        }
        public class GiaKhoaHoc
        {
            public int Gia { get; set; }
        }
    }
}
