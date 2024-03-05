using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        public class DanhMucConDTO
        {
            public int MaDanhMucCon { get; set; }
            public string TieuDeDanhMuc { get; set; }
        }
        private readonly LearningHubContext _contexts;
        private readonly AppSetting _appSettings;
        public DanhMucController(LearningHubContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _contexts = context;
            _appSettings = optionsMonitor.CurrentValue;
        }
        public class GioiThieuKhoaHoc
        {
            public string TieuDeDanhMuc { get; set; }
        }
        [HttpGet]
        [Route("DanhSach-DanhMuc")]
        public IActionResult GetDanhMucChinh()
        {
            var danhMucChinh = _contexts.DanhMucKhoaHocs.ToList();
            return Ok(danhMucChinh);
        }

        [HttpGet]
        [Route("TieuDeLinhVucDaChon/{maNguoiDay}")]
        public IActionResult GetTieuDeLinhVucDaChon(int maNguoiDay)
        {
            var tieuDeVaMaDanhMucLinhVucDaChon = _contexts.LinhVucGiangDays
                .Where(lv => lv.MaNguoiDay == maNguoiDay) // Lọc theo MaNguoiDay
                .SelectMany(lv => _contexts.DanhMucKhoaHocs
                .Where(dmkh => dmkh.MaDanhMuc == lv.MaDanhmuc) // Lọc theo MaDanhMuc
                .Select(dmkh => new
                {
                    MaDanhMuc = dmkh.MaDanhMuc,
                    TieuDeDanhMuc = dmkh.TieuDeDanhMuc
                }) // Lấy cả mã danh mục và tiêu đề danh mục
        )
        .ToList();

            return Ok(tieuDeVaMaDanhMucLinhVucDaChon);
        }

        [HttpGet("{id}/DanhMucCon")]
        public IActionResult GetDanhMucCon(int id)
        {
            var danhMucChinh = _contexts.DanhMucKhoaHocs.FirstOrDefault(d => d.MaDanhMuc == id);
            if (danhMucChinh == null)
            {
                return NotFound();
            }

            var danhMucCon = _contexts.DanhMucCons
                .Where(dc => dc.MaDanhMuc == id)
                .Select(dc => new DanhMucConDTO
                {
                    MaDanhMucCon = dc.MaDanhMucCon,
                    TieuDeDanhMuc = dc.TieuDeDanhMuc
                })
                .ToList();

            return Ok(danhMucCon);
        }
        [HttpPost]
        [Route("Them-danhmuc")]
        public async Task<IActionResult> ThemDanhMuc([FromForm] GioiThieuKhoaHoc model)
        {
            var DanhMucMoi = new DanhMucKhoaHoc
            {
                TieuDeDanhMuc = model.TieuDeDanhMuc,
            };

            _contexts.DanhMucKhoaHocs.Add(DanhMucMoi);
            await _contexts.SaveChangesAsync();
            return Ok(new { message = "Danh mục chính đã được tạo thành công" });
        }
        [HttpGet]
        [Route("GetThongKeAdmin")]
        public async Task<IActionResult> GetThongKeAdmin()
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Giả sử tỷ giá hối đoái từ USD sang VND là 23000
            decimal exchangeRateUSDToVND = 23000;

            var ketquathongke = _contexts.ThanhToans
                .Where(thanhtoan => thanhtoan.NgayThanhToan >= firstDayOfMonth && thanhtoan.NgayThanhToan <= lastDayOfMonth)
                .Select(thanhtoan => new
                {
                    // Chuyển đổi giá từ USD sang VND
                    doanhthu = thanhtoan.SoTienNhanDuoc * exchangeRateUSDToVND
                })
                .ToList();
            decimal? loinhuan = ketquathongke.Sum(item => item.doanhthu);

            int hocVienMoi = _contexts.HocViens
                .Count(hocvien => hocvien.NgayTao >= firstDayOfMonth && hocvien.NgayTao <= lastDayOfMonth);
            
            int giangvienmoi = _contexts.NguoiDays
                .Count(hocvien => hocvien.NgayTao >= firstDayOfMonth && hocvien.NgayTao <= lastDayOfMonth);
            int khoahocmoi = _contexts.KhoaHocs
                .Count(hocvien => hocvien.NgayTao >= firstDayOfMonth && hocvien.NgayTao <= lastDayOfMonth);


            return Ok(new { loinhuan = loinhuan, hocvienmoi = hocVienMoi, nguoihuongdanmoi = giangvienmoi, khoahocmoi = khoahocmoi });
        }


        [HttpGet]
        [Route("GetThongKenguoiday/{manguoiday}")]
        public async Task<IActionResult> GetThongKenguoiday(int manguoiday)
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Giả sử tỷ giá hối đoái từ USD sang VND là 23000
            decimal exchangeRateUSDToVND = 23000;

            var ketquathongke = _contexts.ThanhToans
                .Where(thanhtoan => thanhtoan.NgayThanhToan >= firstDayOfMonth && thanhtoan.NgayThanhToan <= lastDayOfMonth && thanhtoan.MaNguoiDay == manguoiday)
                .Select(thanhtoan => new
                {
                    // Chuyển đổi giá từ USD sang VND
                    doanhthu = thanhtoan.SoTienNhanDuoc * exchangeRateUSDToVND
                })
                .ToList();

            decimal? loinhuan = ketquathongke.Sum(item => item.doanhthu);

            int hocVienMoi = _contexts.DangKyHocs
                .Count(hocvien => hocvien.NgayThanhToan >= firstDayOfMonth && hocvien.NgayThanhToan <= lastDayOfMonth );

            int khoahocmoi = _contexts.KhoaHocs
                .Count(khoahoc => khoahoc.NgayTao >= firstDayOfMonth && khoahoc.NgayTao <= lastDayOfMonth && khoahoc.MaNguoiDay == manguoiday);

            return Ok(new { loinhuan = loinhuan, hocvienmoi = hocVienMoi, khoahocmoi = khoahocmoi });
        }


    }
}
