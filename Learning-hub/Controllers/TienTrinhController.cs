using Learning_hub.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TienTrinhController : ControllerBase
    {
        private readonly LearningHubContext _context;

        public TienTrinhController(LearningHubContext context)
        {
            _context = context;
        }
        public class TienTrinhHocTap
        {
            public string idkhoahoc { get; set; }
            public int tongbaigiang { get; set; }
            public int baigiangdahoc { get; set; }
            public string tieudechuong { get; set; }
            public string tieudebaigiang { get; set; }
        }


        [HttpGet("laytientrinh/{userId}")]
        public IActionResult GetProgress(int userId)
        {
            try
            {
                var userProgress = _context.TienTrinhHocs.SingleOrDefault(p => p.MaHocVien == userId);

                if (userProgress == null)
                {
                    // Trả về giá trị mặc định nếu không có tiến trình học tập cho người dùng này
                    return Ok(new { Chapter = "", Lesson = "" });
                }

                return Ok(new { Chapter = userProgress.TieuDeChuong, Lesson = userProgress.TieuDeBaiGiang });
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                Console.Error.WriteLine($"Lỗi khi lấy tiến trình học tập: {ex.Message}");

                // Trả về mã lỗi và thông điệp lỗi
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("capnhattientrinh/{userId}")]
        public IActionResult UpdateProgress(int userId, [FromBody] TienTrinhHocTap request)
        {
            try
            {
                var userProgress = _context.TienTrinhHocs.SingleOrDefault(p => p.MaHocVien == userId);

                if (userProgress == null)
                {
                    userProgress = new TienTrinhHoc { MaHocVien = userId };
                    _context.TienTrinhHocs.Add(userProgress);
                }

                userProgress.MaKhoaHoc = request.idkhoahoc;
                userProgress.TongBaiGiang = request.tongbaigiang;
                if (userProgress.BaiGiangHocDuoc.HasValue && userProgress.BaiGiangHocDuoc != userProgress.TongBaiGiang)
                {
                    userProgress.BaiGiangHocDuoc += request.baigiangdahoc;
                }
                else
                {
                    // Nếu BaiGiangHocDuoc chưa có giá trị, gán giá trị mới từ request
                    userProgress.BaiGiangHocDuoc = request.baigiangdahoc;
                }
                userProgress.TieuDeChuong = request.tieudechuong;
                userProgress.TieuDeBaiGiang = request.tieudebaigiang;

                _context.SaveChanges();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                Console.Error.WriteLine($"Lỗi khi cập nhật tiến trình học tập: {ex.Message}");

                // Trả về mã lỗi và thông điệp lỗi
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
