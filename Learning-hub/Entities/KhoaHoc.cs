using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_hub.Entities
{
    public partial class KhoaHoc
    {
        public KhoaHoc()
        {
            BaoCaos = new HashSet<BaoCao>();
            CauHois = new HashSet<CauHoi>();
            Chuongs = new HashSet<Chuong>();
            DangKyHocs = new HashSet<DangKyHoc>();
            HoiDaps = new HashSet<HoiDap>();
            KhuyenMaiCuaKhoaHocs = new HashSet<KhuyenMaiCuaKhoaHoc>();
            NhanXets = new HashSet<NhanXet>();
            TienTrinhHocs = new HashSet<TienTrinhHoc>();
        }

        public string MaKhoaHoc { get; set; }
        public int? MaNguoiDay { get; set; }
        public string TieuDeKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public string KienThucThuDuoc { get; set; }
        public string TrinhDo { get; set; }
        public string DanhMuc { get; set; }
        public string DanhMucCon { get; set; }
        public string HinhAnh { get; set; }
        public int? Gia { get; set; }
        public int? SoLuongHocVien { get; set; }
        public string PhanHoi { get; set; }
        public string TinhTrang { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayDuyet { get; set; }

        public virtual NguoiDay MaNguoiDayNavigation { get; set; }
        public virtual ICollection<BaoCao> BaoCaos { get; set; }
        public virtual ICollection<CauHoi> CauHois { get; set; }
        public virtual ICollection<Chuong> Chuongs { get; set; }
        public virtual ICollection<DangKyHoc> DangKyHocs { get; set; }
        public virtual ICollection<HoiDap> HoiDaps { get; set; }
        public virtual ICollection<KhuyenMaiCuaKhoaHoc> KhuyenMaiCuaKhoaHocs { get; set; }
        public virtual ICollection<NhanXet> NhanXets { get; set; }
        public virtual ICollection<TienTrinhHoc> TienTrinhHocs { get; set; }
    }
}
