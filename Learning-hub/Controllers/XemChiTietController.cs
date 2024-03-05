using Learning_hub.Data;
using Learning_hub.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XemChiTietController : ControllerBase
    {
        private readonly LearningHubContext _context;

        public XemChiTietController(LearningHubContext context)
        {
            _context = context;
        }


        [HttpGet("gioithieu/{id}")]
        public IActionResult GetGioiThieu(string id)
        {
            // Chắc chắn rằng id không rỗng hoặc null
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid id");
            }

            var gioiThieu = _context.KhoaHocs
                .Join(
                    _context.NguoiDays,
                    khoaHoc => khoaHoc.MaNguoiDay,
                    nguoiDay => nguoiDay.MaNguoiDay,
                    (khoaHoc, nguoiDay) => new
                    {
                        MaKhoaHoc = khoaHoc.MaKhoaHoc,
                        TieuDeKhoaHoc = khoaHoc.TieuDeKhoaHoc,
                        MoTa = khoaHoc.MoTa,
                        KienThucThuDuoc = khoaHoc.KienThucThuDuoc,
                        TrinhDo = khoaHoc.TrinhDo,
                        DanhMuc = khoaHoc.DanhMuc,
                        DanhMucCon = khoaHoc.DanhMucCon,
                        HinhAnh = khoaHoc.HinhAnh,
                        Gia = khoaHoc.Gia,
                        SoLuongHocVien = khoaHoc.SoLuongHocVien,
                        TenNguoiDay = nguoiDay.TenNguoiDay
                    }
                )
                .FirstOrDefault(item => item.MaKhoaHoc == id);

            if (gioiThieu == null)
            {
                return NotFound("Không tìm thấy thông tin cho id đã cho");
            }

            return Ok(gioiThieu);
        }
        public class PhanHoiModel
        {
            public string PhanHoi { get; set; }
            public string TinhTrang { get; set; }
        }

        [HttpPut("Putpheduyet/{makhoahoc}")]
        public IActionResult Putpheduyet(string makhoahoc, [FromBody] PhanHoiModel phanHoiModel)
        {
            if (phanHoiModel == null)
            {
                return BadRequest("Dữ liệu phản hồi không hợp lệ");
            }

            var khoaHoc = _context.KhoaHocs.FirstOrDefault(kh => kh.MaKhoaHoc == makhoahoc);

            if (khoaHoc == null)
            {
                return NotFound("Không tìm thấy khóa học");
            }

            string phanhoi = phanHoiModel.PhanHoi;

            if (phanhoi == null)
            {
                return BadRequest("Không có phản hồi");
            }

            khoaHoc.TinhTrang = phanHoiModel.TinhTrang;
            khoaHoc.PhanHoi = phanhoi;
            khoaHoc.NgayDuyet = DateTime.Now;
            _context.SaveChanges();

            return Ok("Cập nhật trạng thái thành công");
        }

        [HttpGet("chuongtrinh/{makhoahoc}")]
        public async Task<IActionResult> GetChuongTrinh(string makhoahoc)
        {
            try
            {

                var khoaHoc = await _context.KhoaHocs
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
                    Chuongs = khoaHoc.Chuongs.Select(c => new
                    {
                        TieuDeChuong = c.TieuDeChuong,
                        BaiGiangs = c.BaiGiangs.Select(bg => new
                        {
                            TieuDeBaiGiang = bg.TieuDeBaiGiang,
                            ThoiLuong = bg.ThoiLuong,
                            Video = bg.Video,
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

        [HttpGet]
        [Route("GetCauHoiByMaKhoaHoc/{maKhoaHoc}")]
        public IActionResult GetCauHoiByMaKhoaHoc(string maKhoaHoc)
        {
            var cauHoiTheoKhoaHoc = _context.CauHois.Where(ch => ch.MaKhoaHoc == maKhoaHoc).ToList();
            return Ok(cauHoiTheoKhoaHoc);
        }

        public class CauHoiDap
        {
            public string maKhoaHoc { get; set; }
            public int maHocVien { get; set; }
            public string tennguoigui { get; set; }
            public string tieuDe { get; set; }
            public string moTa { get; set; }
            public DateTime ngayTao { get; set; }


        }
        public class Cautraloi
        {
            public int mahoidap { get; set; }
            public string tennguoitraloi { get; set; }
            public string moTa { get; set; }

        }

        [HttpPost]
        [Route("Posthoidap")]
        public IActionResult Posthoidap( CauHoiDap hoidap)
        {
            try
            {

                var ketqua = new HoiDap
                {
                    MaKhoaHoc = hoidap.maKhoaHoc,
                    MaHocVien = hoidap.maHocVien,
                    TenNguoiGui = hoidap.tennguoigui,
                    TieuDe = hoidap.tieuDe,
                    MoTa = hoidap.moTa,
                    NgayTao = hoidap.ngayTao,
                };
                _context.HoiDaps.Add(ketqua);
                _context.SaveChanges();

                return Ok("thêm hỏi đáp thành công");
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Posttrahoidap")]
        public IActionResult Posttrahoidap(Cautraloi traloi)
        {
            try
            {

                var ketqua = new TraLoiHoiDap
                {
                    MaHoiDap = traloi.mahoidap,
                    TenNguoiTraLoi = traloi.tennguoitraloi,
                    MoTa = traloi.moTa,
                    NgayTao = DateTime.Now,
                };
                _context.TraLoiHoiDaps.Add(ketqua);
                _context.SaveChanges();

                return Ok("thêm hỏi đáp thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("Gethoidap/{makhoahoc}")]
        public IActionResult Gethoidap(string makhoahoc)
        {
            var query = (from hd in _context.HoiDaps
                        join KhoaHoc in _context.KhoaHocs on hd.MaKhoaHoc equals KhoaHoc.MaKhoaHoc
                         where hd.MaKhoaHoc == makhoahoc 
                        select new
                        {
                            hd.TieuDe,
                            hd.MoTa,
                            hd.NgayTao,
                            hd.TenNguoiGui,
                            hd.MaHoiDap,
                             CauTraLoi = (from ctl in _context.TraLoiHoiDaps
                                          where ctl.MaHoiDap == hd.MaHoiDap
                                          select new
                                          {
                                              ctl.MoTa,
                                              ctl.NgayTao,
                                              ctl.TenNguoiTraLoi
                                          }).ToList()
                        }).ToList();

            return Ok(query);
        }



    }
}
