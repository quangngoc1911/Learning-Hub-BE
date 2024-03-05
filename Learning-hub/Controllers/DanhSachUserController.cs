using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhSachUserController : ControllerBase
    {
        private readonly LearningHubContext _contexts;
        private readonly AppSetting _appSettings;
        public DanhSachUserController(LearningHubContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _contexts = context;
            _appSettings = optionsMonitor.CurrentValue;
        }
        [HttpGet]
        [Route("DanhSach-NguoiDay")]
        public IActionResult GetDanhSachNguoiDay([FromQuery] string tinhTrang)
        {
            //var danhMucChinh = _contexts.NguoiDays.ToList();

            //var filteredDanhMucChinh = danhMucChinh
            //    .Where(nguoiDay => nguoiDay.Role == "nguoiday") // Chỉ lấy người dạy
            //    .Where(nguoiDay =>
            //        tinhTrang == null || nguoiDay.TinhTrang == tinhTrang) // Lọc theo tình trạng (nếu được cung cấp)
            //    .ToList();
            var thongtinnguoiday = from nguoiDay in _contexts.NguoiDays
                    where nguoiDay.TinhTrang == tinhTrang
                    select new
                    {
                        maNguoiDay = nguoiDay.MaNguoiDay,
                        tenNguoiDay = nguoiDay.TenNguoiDay,
                        gioithieubanthan = nguoiDay.GioiThieuBanThan,
                        hinhanh = nguoiDay.HinhAnh,
                        email = nguoiDay.Email,
                        ngaySinh = nguoiDay.NgaySinh,
                        cccd = nguoiDay.Cccd,
                        tinhTrang = nguoiDay.TinhTrang,
                        linhVucGiangDay = _contexts.LinhVucGiangDays
                            .Where(linhVuc => linhVuc.MaNguoiDay == nguoiDay.MaNguoiDay)
                            .Select(linhVuc => new
                            {
                                maLinhVuc = linhVuc.MaLinhVuc,
                                tieuDeDanhMuc = _contexts.DanhMucKhoaHocs
                                    .Where(danhMuc => danhMuc.MaDanhMuc == linhVuc.MaDanhmuc)
                                    .Select(danhMuc => danhMuc.TieuDeDanhMuc)
                                    .FirstOrDefault(),
                                // Thêm các trường khác nếu cần
                            })
                            .ToList(),
                        // Thêm các trường khác nếu cần
                    };

            return Ok(thongtinnguoiday);
        }

        [HttpGet]
        [Route("DanhSach-HocVien")]
        public IActionResult GetDanhSachHocVien()
        {

            var thongtinhocvien = from hocvien in _contexts.HocViens
                                  join DangKy in _contexts.DangKyHocs on hocvien.MaHocVien equals DangKy.MaHocVien into gj
                                  from subDangKy in gj.DefaultIfEmpty()
                                  group subDangKy by new
                                  {
                                      hocvien.TenHocVien,
                                      hocvien.NgaySinh,
                                      hocvien.Email,
                                  } into grp
                                  select new
                                  {
                                      TenHocVien = grp.Key.TenHocVien,
                                      NgaySinh = grp.Key.NgaySinh.HasValue ? grp.Key.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                                      Email = grp.Key.Email,
                                      KhoaHocDaMua = grp.Count(), // Count the number of related entries
                                  };

            return Ok(thongtinhocvien);
        }



        [HttpGet]
        [Route("DanhSach-NguoiDay-Client")]
        public IActionResult GetDanhSachNguoiDayClient()
        {
            var danhMucChinh = _contexts.NguoiDays.ToList();

            var filteredDanhMucChinh = danhMucChinh
                .Where(nguoiDay => nguoiDay.Role == "nguoiday") 
                .Where(nguoiDay => nguoiDay.TinhTrang == "daduyet") 
                .Select(nguoiDay => new
                {
                    maNguoiDay = nguoiDay.MaNguoiDay,
                    tenNguoiDay = nguoiDay.TenNguoiDay,
                    gioithieubanthan = nguoiDay.GioiThieuBanThan,
                    hinhanh = nguoiDay.HinhAnh,
                    linhVucGiangDay = _contexts.LinhVucGiangDays
                    .Where(linhVucGiangDay => linhVucGiangDay.MaNguoiDay == nguoiDay.MaNguoiDay)
                    .Join(_contexts.DanhMucKhoaHocs,
                           lv => lv.MaDanhmuc,
                           dm => dm.MaDanhMuc,
                           (lv, dm) => dm.TieuDeDanhMuc
                     )
                     .ToList(),

                })
                .ToList();

            return Ok(filteredDanhMucChinh);
        }

        [HttpGet]
        [Route("GetChiTietNguoiDay/{manguoiday}")]
        public IActionResult GetChiTietNguoiDay(int manguoiday)
        {
            var nguoiday = _contexts.NguoiDays
                            .FirstOrDefault(nd => nd.MaNguoiDay == manguoiday
                            && nd.Role == "nguoiday"
                            && nd.TinhTrang == "daduyet");

            var linhvuc = from nguoiDay in _contexts.NguoiDays
                          join LinhVucGiangDay in _contexts.LinhVucGiangDays on nguoiDay.MaNguoiDay equals LinhVucGiangDay.MaNguoiDay
                          join DanhMucKhoaHoc in _contexts.DanhMucKhoaHocs on LinhVucGiangDay.MaDanhmuc equals DanhMucKhoaHoc.MaDanhMuc
                          where nguoiDay.MaNguoiDay == manguoiday
                          select new
                          {
                              LinhVucGiangDay = DanhMucKhoaHoc.TieuDeDanhMuc,
                          };

            var thongtinnguoiday = new
            {
                maNguoiDay = nguoiday.MaNguoiDay,
                tenNguoiDay = nguoiday.TenNguoiDay,
                gioithieubanthan = nguoiday.GioiThieuBanThan,
                hinhanh = nguoiday.HinhAnh,
            };

            // Đóng kết nối sau truy vấn người dạy


            var khoahoc = from KhoaHoc in _contexts.KhoaHocs
                          join NguoiDay in _contexts.NguoiDays on KhoaHoc.MaNguoiDay equals NguoiDay.MaNguoiDay
                          //join NhanXet in _contexts.NhanXets on KhoaHoc.MaKhoaHoc equals NhanXet.MaKhoaHoc into nh
                          //from NhanXet in nh.DefaultIfEmpty()  // left join
                          where KhoaHoc.MaNguoiDay == manguoiday
                          where KhoaHoc.TinhTrang == "daduyet"
                          select new
                          {
                              MaKhoaHoc = KhoaHoc.MaKhoaHoc,
                              TenKhoaHoc = KhoaHoc.TieuDeKhoaHoc,
                              TenNguoiDay = NguoiDay.TenNguoiDay,
                              SoLuongHocVien = KhoaHoc.SoLuongHocVien,
                             // DiemNhanXet = NhanXet != null ? NhanXet.DiemNhanXet : 0,  // Thay "Giá trị mặc định" bằng giá trị mặc định mong muốn
                              HinhAnh = KhoaHoc.HinhAnh,
                          };

            // Đóng kết nối sau truy vấn khóa học
            var khoahocData = khoahoc.ToList();

            var thongtin = new
            {
                linhvuc = linhvuc,
                thongtinnguoiday,
                khoahoc = khoahocData,
            };

            return Ok(thongtin);
        }



        [HttpGet]
        [Route("Getnoidungkhoahoc/{makhoahoc}")]
        public async Task<IActionResult> Getnoidungkhoahoc( string makhoahoc)
        {
            try
            {

                var khoaHoc = await _contexts.KhoaHocs
                    .Where(kh => kh.MaKhoaHoc == makhoahoc)
                    .Include(kh => kh.Chuongs)
                    .ThenInclude(c => c.BaiGiangs)
                    .FirstOrDefaultAsync();

                if (khoaHoc == null)
                {
                    return NotFound("Không tìm thấy khóa học.");
                }

                var result = new
                {
                    TieuDe = khoaHoc.TieuDeKhoaHoc,
                    MaKhoaHoc = khoaHoc.MaKhoaHoc,
                    Chuongs = khoaHoc.Chuongs.Select(c => new
                    {
                        TieuDeChuong = c.TieuDeChuong,
                        BaiGiangs = c.BaiGiangs.Select(bg => new
                        {
                            TieuDeBaiGiang = bg.TieuDeBaiGiang,
                            ThoiLuong = bg.ThoiLuong,
                            Video = bg.Video,
                            XemTruoc = bg.XemTruocVideo,
                            FileTaiLieu = bg.FileTaiLieu
                        })
                    })
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("pheduyetnguoiday/{manguoiday}")]
        public async Task<IActionResult> pheduyetnguoiday(int manguoiday)
        {
            var nguoiDay = await _contexts.NguoiDays.FirstOrDefaultAsync(x => x.MaNguoiDay == manguoiday);

            if (nguoiDay == null)
            {
                return NotFound(); // Hoặc BadRequest() nếu muốn trả về mã lỗi khác
            }

            nguoiDay.TinhTrang = "daduyet"; // Đặt trạng thái đã duyệt là true

            try
            {
                await _contexts.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return Ok(); // Trả về 200 OK nếu thành công
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Lỗi khi cập nhật cơ sở dữ liệu."); // Hoặc BadRequest() nếu muốn trả về mã lỗi khác
            }
        }






    }
}
