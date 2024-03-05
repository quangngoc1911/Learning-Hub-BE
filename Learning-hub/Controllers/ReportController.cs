using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Learning_hub.Data;
using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Learning_hub.Controllers.ReportController;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly LearningHubContext _contexts;



        public ReportController(LearningHubContext context)
        {
            _contexts = context;
        }
        public class report {
            public string makhoahoc { get; set; }
            public int mahocvien { get; set; }
            public string chitiet { get; set; }
            public string tieude { get; set; }
            public string mota { get; set; }
            public DateTime ngaytao { get; set; }
            public IFormFile hinhanh { get; set; }

        }

        [HttpPost]
        [Route("Postreport")]
        public async Task<IActionResult> Postreport([FromForm]report rp)
        {
            Account account = new Account("ddof16h09", "975568459733683", "wDD3JgPxewySR6tAnswwU3aqjfQ");
            Cloudinary cloudinary = new Cloudinary(account);

            if (rp.hinhanh != null && rp.hinhanh.Length > 0)
            {

                var result = cloudinary.UploadLarge(new ImageUploadParams
                {
                    File = new FileDescription(rp.hinhanh.FileName, rp.hinhanh.OpenReadStream()),
                    PublicId = "test"
                });
                var imageUrl = result.Url.ToString(); // Đường dẫn ảnh trên Cloudinary
                try {
                    string tinhtrang = "chuaduyet";
                    var ketqua = new BaoCao
                    {
                        MaKhoaHoc = rp.makhoahoc,
                        MaHocVien = rp.mahocvien,
                        TieuDe = rp.tieude,
                        ChiTietBaoCao = rp.chitiet,
                        MoTa = rp.mota,
                        NgayTao = rp.ngaytao,
                        HinhAnh = imageUrl,
                        TinhTrang = tinhtrang
                    };
                    _contexts.BaoCaos.Add(ketqua);
                    _contexts.SaveChanges();
                    return Ok(new { message = "Cảm ơn bạn đã gửi báo cáo" });
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return BadRequest("lỗi khi thêm báo cáo");


        }


        [HttpGet]
        [Route("Getreport/{tinhtrang}")]
        public async Task<IActionResult> Getreport(string tinhtrang)
        {
            try {

                var query = await (from bc in _contexts.BaoCaos
                                   join hv in _contexts.HocViens on bc.MaHocVien equals hv.MaHocVien
                                   join kh in _contexts.KhoaHocs on bc.MaKhoaHoc equals kh.MaKhoaHoc
                                   where bc.TinhTrang == tinhtrang
                                   select new
                                   {
                                       bc.MaBaoCao,
                                       bc.MaKhoaHoc,
                                       bc.MaHocVien,
                                       bc.ChiTietBaoCao,
                                       bc.TieuDe,
                                       bc.MoTa,
                                       bc.HinhAnh,
                                       bc.TinhTrang,
                                       bc.NgayTao,
                                       HocVien = new
                                       {
                                           hv.TenHocVien
                                       },
                                       kh.TieuDeKhoaHoc
                                   }).ToListAsync();


                return Ok(query);

                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        public class pheduyet
        {
            public string phanhoi { get; set; }
            public int mabaocao { get; set; }
        }

        [HttpPut]
        [Route("Putreport")]
        public async Task<IActionResult> Putreport(pheduyet pd)
        {
            try
            {

                var report = await _contexts.BaoCaos.FirstOrDefaultAsync(r => r.MaBaoCao == pd.mabaocao);

                if (report == null)
                {
                    // Nếu không tìm thấy báo cáo, trả về NotFound hoặc BadRequest tùy theo yêu cầu của bạn
                    return NotFound($"Không tìm thấy báo cáo với mã {pd.mabaocao}");
                }

                
                report.PhanHoi = pd.phanhoi;
                report.TinhTrang = "daduyet";
                // Lưu thay đổi vào cơ sở dữ liệu
                await _contexts.SaveChangesAsync();



                return Ok("phê duyệt phản hồi thành công");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }



        public class nhanxet
        {
            public string MaKhoaHoc { get; set; }
            public string TeNguoiGui { get; set; }
            public string NoiDung { get; set; }
            public int SoDiem { get; set; }
            public string phanhoi { get; set; }
        }

        [HttpPost]
        [Route("Postbaocao")]
        public async Task<IActionResult> Postbaocao(nhanxet dg)
        {
            try
            {
                using (var context = new LearningHubContext(/*...inject DbContextOptions if needed...*/))
                {
                    // Gán dữ liệu từ dg vào một đối tượng DanhGia mới
                    var danhGiaEntity = new NhanXet
                    {
                        MaKhoaHoc = dg.MaKhoaHoc,
                        TenNguoiGui = dg.TeNguoiGui,
                        NoiDung = dg.NoiDung,
                        DiemNhanXet = dg.SoDiem,
                        NgayTao = DateTime.Now,
                    };

                    // Thêm đối tượng mới vào DbSet
                    context.NhanXets.Add(danhGiaEntity);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await context.SaveChangesAsync();
                }

                return Ok("Thêm đánh giá thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("Getbaocao/{makhoahoc}")]
        public async Task<IActionResult> Getbaocao(string makhoahoc)
        {
            try
            {
                var query = await (from bc in _contexts.NhanXets
                                   where bc.MaKhoaHoc == makhoahoc 
                                   select new
                                   {
                                       TeNguoiGui = bc.TenNguoiGui,
                                       NoiDung = bc.NoiDung,
                                       DiemNhanXet = bc.DiemNhanXet,
                                       NgayTao = bc.NgayTao
                                   }).ToListAsync();

                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }






    }
}
