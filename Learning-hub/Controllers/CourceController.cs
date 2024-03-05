using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Learning_hub.Data;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Net;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Learning_hub.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static Learning_hub.Controllers.CourceController;
using static Learning_hub.Models.KhoaHoc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourceController : ControllerBase
    {
       

        private readonly LearningHubContext _context;
        private readonly AppSetting _appSettings;
  
        public CourceController(LearningHubContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("gioi-thieu")]
        public async Task<IActionResult> Update([FromForm] TaoKhoaHocModel.GioiThieuKhoaHoc model)
        {
            Account account = new Account("ddof16h09", "975568459733683", "wDD3JgPxewySR6tAnswwU3aqjfQ");
            Cloudinary cloudinary = new Cloudinary(account);

            if (model.HinhAnh == null)
            {
                // Xử lý trường hợp file là null ở đây
                return BadRequest("File is null.");
            }

            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.HinhAnh.FileName, model.HinhAnh.OpenReadStream()),
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var imageUrl = uploadResult.SecureUri.ToString(); // Lấy đường dẫn ảnh từ Cloudinary

                    var newGioiThieu = new Entities.KhoaHoc
                    {
                        MaKhoaHoc = model.MaKhoaHoc,
                        MaNguoiDay = model.MaNguoiDay,
                        TieuDeKhoaHoc = model.TieuDeKhoaHoc,
                        MoTa = model.MoTa,
                        KienThucThuDuoc = model.KienThucThuDuoc,
                        TrinhDo = model.TrinhDo,
                        DanhMuc = model.DanhMuc,
                        DanhMucCon = model.DanhMucCon,
                        NgayTao = DateTime.Now,
                        HinhAnh = imageUrl, 
                        TinhTrang = model.TinhTrang,
                    };

                    _context.KhoaHocs.Add(newGioiThieu);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Cập nhật giới thiệu khóa học thành công" });
                }
                else
                {
                    return BadRequest("Upload ảnh thất bại");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("chuongTrinh")]
        public async Task<IActionResult> SaveCourseContent(CourseData courseData)
        {
            try
            {
                foreach (var section in courseData.formattedData)
                {
                    var newChuong = new Entities.Chuong
                    {
                        MaKhoaHoc = courseData.MaKhoaHoc,
                        TieuDeChuong = section.Title
                    };

                    _context.Chuongs.Add(newChuong);
                    await _context.SaveChangesAsync();

                    foreach (var lesson in section.Lessons)
                    {
                        var newBaiGiang = new Entities.BaiGiang
                        {
                            TieuDeBaiGiang = lesson.title,
                            ThoiLuong = lesson.thoiluong,
                            Video = lesson.Video,
                            FileTaiLieu = lesson.File,
                            XemTruocVideo = lesson.xemtruoc,
                            MaChuong = newChuong.MaChuong
                        };

                        _context.BaiGiangs.Add(newBaiGiang);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok("Dữ liệu đã được lưu thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest("Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("cauhoi")]
        public async Task<IActionResult> SaveCauHoi([FromBody] CauHoiRequest request)
        {
            try
            {
                foreach (var question in request.questions)
                {
                    var newQuestion = new CauHoi
                    {
                        MaKhoaHoc = request.maKhoaHoc,
                        CauHoi1 = question.TieuDeCauHoi,
                        DapAn1 = question.DapAns[0].TieuDeDapAn,
                        DapAn2 = question.DapAns[1].TieuDeDapAn,
                        DapAn3 = question.DapAns[2].TieuDeDapAn,
                        DapAn4 = question.DapAns[3].TieuDeDapAn,
                        DapAnDung = question.DapAnDung
                    };

                    // Lưu newQuestion vào database
                    _context.CauHois.Add(newQuestion);
                    await _context.SaveChangesAsync();
                }

                // Trả về thành công hoặc thông báo khác tùy vào kết quả xử lý
                return Ok("Dữ liệu đã được thêm thành công.");
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                Console.WriteLine($"Lỗi khi xử lý yêu cầu: {ex.ToString()}");

                // Trả về mã lỗi HTTP 500 - Internal Server Error
                return StatusCode(500, "Đã xảy ra lỗi nội bộ. Vui lòng thử lại sau.");
            }
        }

        [HttpPut]
        [Route("giakhoahoc")]
        public async Task<IActionResult> updateGia([FromBody] GiaKhoaHoc giakhoahoc)
        {
            try
            {
                    var khoaHoc = _context.KhoaHocs.FirstOrDefault(kh => kh.MaKhoaHoc == giakhoahoc.maKhoaHoc);

                    if (khoaHoc != null)
                    {
                        // Cập nhật giá
                        khoaHoc.Gia = giakhoahoc.giakhoahoc;

 
                        await _context.SaveChangesAsync();
                    return Ok("Giá khóa học đã được cập nhật thành công.");
                    }
                    else
                    {
                        return NotFound("Không tìm thấy khóa học.");
                    }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Lỗi khi cập nhật giá khóa học: {ex.ToString()}");
                return StatusCode(500, "Đã xảy ra lỗi nội bộ. Vui lòng thử lại sau.");
            }

        }

        [HttpGet]
        [Route("ChiTietKhoaHoc/{makhoahoc}")]
        public async Task<IActionResult> GetChiTietKhoaHoc(string makhoahoc)
        {
            try
            {
                var khoahoc = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into km
                                    from khuyenmai in km.DefaultIfEmpty()
                                    where khoaHoc.MaKhoaHoc == makhoahoc
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        MoTa = khoaHoc.MoTa,
                                        KienThucThuDuoc = khoaHoc.KienThucThuDuoc,
                                        TrinhDo = khoaHoc.TrinhDo,
                                        DanhMuc = khoaHoc.DanhMuc,
                                        Gia = khoaHoc.Gia,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        DanhMucCon = khoaHoc.DanhMucCon,
                                        MaNguoiDay = khoaHoc.MaNguoiDay,
                                        TenNguoiday = nguoiDay.TenNguoiDay,
                                        giadagiam = khuyenmai != null ? khuyenmai.GiaKhiGiam : null,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                    };

                var baigiang = await _context.KhoaHocs
                    .Where(kh => kh.MaKhoaHoc == makhoahoc)
                    .Include(kh => kh.Chuongs)
                    .ThenInclude(c => c.BaiGiangs)
                    .FirstOrDefaultAsync();
                var ketqua = new
                {
                    Chuongs = baigiang?.Chuongs?.Select(c => new
                    {
                        SoLuongBaiGiang = c?.BaiGiangs?.Count ?? 0,
                        //TongThoiLuong = c?.BaiGiangs?.Sum(bg => bg?.ThoiLuong ?? 0) ?? 0,
                        //BaiGiangs = c?.BaiGiangs?.Select(bg => new
                        //{
                        //    ThoiLuong = bg?.ThoiLuong ?? 0,
                        //}) ?? Enumerable.Empty<object>(),
                    }) ?? Enumerable.Empty<object>(),
                    TongThoiLuongToanKhoaHoc = baigiang?.Chuongs?.Sum(c => c?.BaiGiangs?.Sum(bg => bg?.ThoiLuong ?? 0) ?? 0) ?? 0,
                };

                if (baigiang == null)
                {
                    return NotFound();
                }
                var result = new
                {
                    
                    KhoaHoc = khoahoc,
                    baigiang = ketqua,
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("ChiTietNguoiDay/{manguoiday}")]
        public async Task<IActionResult> GetThongTinNguoiDay(int manguoiday)
        {
            try
            {
                var nguoiDayInfo = from  nguoiDay in _context.NguoiDays
                                   where nguoiDay.MaNguoiDay == manguoiday
                                   select new
                                   {
                                       TenNguoiDay = nguoiDay.TenNguoiDay,
                                       GioiThieuBanThan = nguoiDay.GioiThieuBanThan,
                                       //LinhVucGiangDay = nguoiDay.LinhVucGiangDays,
                                       HinhAnh = nguoiDay.HinhAnh
                                   };
                if (nguoiDayInfo == null)
                {
                    return NotFound();
                }
                var result = new
                {

                    nguoiday = nguoiDayInfo
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("LinhVucNguoiDay/{manguoiday}")]
        public async Task<IActionResult> GetLinhVuc(int manguoiday)
        {
            try
            {
                var nguoiDayInfo = from nguoiDay in _context.NguoiDays
                                   join LinhVucGiangDay in _context.LinhVucGiangDays on nguoiDay.MaNguoiDay equals LinhVucGiangDay.MaNguoiDay
                                   join DanhMucKhoaHoc in _context.DanhMucKhoaHocs on LinhVucGiangDay.MaDanhmuc equals DanhMucKhoaHoc.MaDanhMuc
                                   where nguoiDay.MaNguoiDay == manguoiday
                                   select new
                                   {

                                       LinhVucGiangDay = DanhMucKhoaHoc.TieuDeDanhMuc,
                                   };
                if (nguoiDayInfo == null)
                {
                    return NotFound();
                }
                var result = new
                {

                    nguoiday = nguoiDayInfo
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        public class mota
        {
            public string idkhoahoc { get; set; }
        }


        [HttpGet]
        [Route("Getmota/{mahocvien}/{makhoahoc}")]
        public async Task<IActionResult> Getmota(int mahocvien, string makhoahoc)
        {
            try
            {
                if (!string.IsNullOrEmpty(makhoahoc))
                { // Nếu có idkhoahoc, lọc theo mã khóa học
                    var moTa = (from dangKyHoc in _context.DangKyHocs
                                join khoaHoc in _context.KhoaHocs on dangKyHoc.MaKhoaHoc equals khoaHoc.MaKhoaHoc
                                where dangKyHoc.MaKhoaHoc == khoaHoc.MaKhoaHoc
                                where dangKyHoc.MaHocVien == mahocvien
                                where dangKyHoc.TinhTrang == "thanhcong"
                                select khoaHoc.MoTa).FirstOrDefault();

                    if (moTa == null)
                    {
                        return NotFound("Không tìm thấy mô tả cho khóa học có mã " + makhoahoc);
                    }

                    return Ok(new { MoTa = moTa });
                }

                

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        [HttpGet]
        [Route("khoahoccuatoi/{mahocvien}")]
        public async Task<IActionResult> GetKhoaHocCuaToi(int mahocvien)
        {
            try
            {
 

                var khoaHocDaMua = (from dangKyHoc in _context.DangKyHocs
                                    join khoaHoc in _context.KhoaHocs on dangKyHoc.MaKhoaHoc equals khoaHoc.MaKhoaHoc
                                    //join tientrinh in _context.TienTrinhHocs on khoaHoc.MaKhoaHoc equals tientrinh.MaKhoaHoc
                                    join hocvien in _context.HocViens on dangKyHoc.MaHocVien equals hocvien.MaHocVien
                                    where dangKyHoc.MaKhoaHoc == khoaHoc.MaKhoaHoc
                                    where dangKyHoc.MaHocVien == mahocvien
                                    where dangKyHoc.TinhTrang == "thanhcong"
                                    //where tientrinh.MaKhoaHoc == khoaHoc.MaKhoaHoc
                                    select new
                                    {
                                        Makhoahoc = khoaHoc.MaKhoaHoc,
                                        TenHocVien = hocvien.TenHocVien,
                                        TieuDeKhoaHoc = khoaHoc.TieuDeKhoaHoc,
                                        MoTa = khoaHoc.MoTa,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        HocPhi = dangKyHoc.HocPhi,
                                        NgayThanhToan = dangKyHoc.NgayThanhToan,
                                        //TongBaiGiang = tientrinh.TongBaiGiang,
                                        //BaiGiangHocDuoc = tientrinh.BaiGiangHocDuoc,
                                        tientrinhhoc = (from tientrinh in _context.TienTrinhHocs
                                                        join hocvien in _context.HocViens on dangKyHoc.MaHocVien equals hocvien.MaHocVien
                                                        where dangKyHoc.MaHocVien == mahocvien
                                                        where tientrinh.MaKhoaHoc == khoaHoc.MaKhoaHoc
                                                        select new
                                                        {
                                                            TongBaiGiang = tientrinh.TongBaiGiang,
                                                            BaiGiangHocDuoc = tientrinh.BaiGiangHocDuoc,
                                                        }).ToList(),
                                    }).ToList();

              
                if (!khoaHocDaMua.Any())
                {
                    return NotFound("Người học chưa đăng ký khóa học nào.");
                }

                var result = new
                {
                    KhoaHocDaMua = khoaHocDaMua,
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("chitietkhoahoccuatoi/{makhoahoc}")]
        public async Task<IActionResult> GetChiTietKhoaHocCuaToi(string makhoahoc)
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
                    MaKhoaHoc = khoaHoc.MaKhoaHoc,
                    Chuongs = khoaHoc.Chuongs.Select(c => new
                    {
                        Machuong = c.MaChuong,
                        TieuDeChuong = c.TieuDeChuong,
                        BaiGiangs = c.BaiGiangs.Select(bg => new
                        {
                            Mabaigiang = bg.MaBaiGiang,
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
                return BadRequest(new { errorMessage = "Có lỗi xảy ra khi xử lý yêu cầu.", exceptionMessage = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetKhoaHocMoi")]
        public async Task<IActionResult> GetKhoaHocMoi()
        {
            try
            {
                DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var khoaHocMoi = await _context.KhoaHocs
                    .Where(kh => kh.NgayTao >= startOfMonth)
                    .OrderByDescending(kh => kh.NgayTao)
                    .Take(8)
                    .Select(kh => new KhoaHocMoi
                    {
                        MaKhoaHoc = kh.MaKhoaHoc,
                        TieuDeKhoaHoc = kh.TieuDeKhoaHoc,
                        TrinhDo = kh.TrinhDo,
                        HinhAnh = kh.HinhAnh,
                        Gia = kh.Gia,
                        SoLuongHocVien = kh.SoLuongHocVien,

                    })
                    .ToListAsync();

                return Ok(khoaHocMoi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetKhoaHocTieuBieu")]
        public async Task<IActionResult> GetKhoaHocTieuBieu()
        {
            try
            {
                DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var khoaHocTieuBieu = await _context.KhoaHocs
                    .Where(kh =>  kh.SoLuongHocVien >= 50)
                    .OrderByDescending(kh => kh.NgayTao)
                    .Take(8)
                    .Select(kh => new KhoaHocMoi
                    {
                        MaKhoaHoc = kh.MaKhoaHoc,
                        TieuDeKhoaHoc = kh.TieuDeKhoaHoc,
                        HinhAnh = kh.HinhAnh,
                        Gia = kh.Gia,
                        SoLuongHocVien = kh.SoLuongHocVien,
                    })
                    .ToListAsync();

                return Ok(khoaHocTieuBieu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }






        //[HttpPost("cauhoi")]
        //public IActionResult AddQuestion([FromBody] Question question)
        //{
        //    questions.Add(question);
        //    return Ok("Câu hỏi đã được thêm thành công.");
        //}


        //[HttpPost]
        //[Route("upload-video")]
        //public async Task<IActionResult> UploadVideo( IFormFile videoFile)
        //{
        //    try
        //    {
        //        Account account = new Account("ddof16h09", "975568459733683", "wDD3JgPxewySR6tAnswwU3aqjfQ");
        //        Cloudinary cloudinary = new Cloudinary(account);

        //        if (videoFile != null && videoFile.Length > 0)
        //        {
        //            // Đọc dữ liệu từ IFormFile và tải lên Cloudinary
        //            using (var stream = videoFile.OpenReadStream())
        //            {
        //                var uploadParams = new VideoUploadParams
        //                {
        //                    File = new FileDescription(videoFile.FileName, stream),
        //                    //PublicId = Guid.NewGuid().ToString(),
        //                    Folder = "samples/video"
        //                };

        //                var uploadResult = cloudinary.Upload(uploadParams);

        //                if (uploadResult.StatusCode == HttpStatusCode.OK)
        //                {
        //                    var videoUrl = uploadResult.Uri.ToString();
        //                    return Ok(new { VideoUrl = videoUrl });
        //                }
        //            }
        //        }

        //        return BadRequest("Invalid video file.");
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //    return BadRequest("không tải được file");
        //}


        //[HttpPost]
        //[Route("chuongTrinh")]
        //public async Task<IActionResult> UpdateChuongTrinh(CourseData courseData, string MaKhoaHoc)
        //{
        //    //if (courseData == null || !courseData.Any())
        //    //{
        //    //    return BadRequest("Dữ liệu không hợp lệ.");
        //    //}

        //    foreach (var section in courseData)
        //    {
        //        var newChuong = new Entities.Chuong
        //        {
        //            MaKhoaHoc = MaKhoaHoc,
        //            TieuDeChuong = section.Title
        //        };
        //        _context.Chuongs.Add(newChuong);
        //        await _context.SaveChangesAsync(); // Lưu thông tin chương trước

        //        foreach (var lesson in section.Lessons)
        //        {
        //            var newBaiGiang = new Entities.BaiGiang
        //            {
        //                TieuDeBaiGiang = lesson.title,
        //                Video = lesson.Video,
        //                FileTaiLieu = lesson.File,
        //                MaChuong = newChuong.MaChuong // Liên kết với bảng Chuong
        //            };
        //            _context.BaiGiangs.Add(newBaiGiang);
        //        }

        //        await _context.SaveChangesAsync(); // Lưu thông tin bài giảng
        //    }
        //    return Ok("Dữ liệu đã được thêm vào cơ sở dữ liệu.");
        //}

        //[HttpPost]
        //[Route("chuongTrinhtest/{testtest}")]
        //public async Task<IActionResult> UpdateChuongTrinhtest(test testtest)
        //{
        //            var newChuong = new Entities.Chuong
        //            {
        //                TieuDeChuong = testtest.tenchuong,
        //                MaKhoaHoc = testtest.MaKhoaHoc
        //            };
        //            _context.Chuongs.Add(newChuong);


        //        await _context.SaveChangesAsync(); // Lưu thông tin bài giảng

        //    return Ok("Dữ liệu đã được thêm vào cơ sở dữ liệu.");
        //}




    }
}
