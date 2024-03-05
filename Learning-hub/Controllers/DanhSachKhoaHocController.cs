using CloudinaryDotNet;
using Learning_hub.Data;
using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhSachKhoaHocController : ControllerBase
    {
        private readonly LearningHubContext _context;

        public DanhSachKhoaHocController(LearningHubContext context)
        {
            _context = context;
        }
   

        [HttpGet]
        [Route("alldanhsach")]
        public IActionResult GetKhoaHoc(int page)
        {
            try {
                int pageSize = 10; // Số sản phẩm trên mỗi trang

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into km
                                    from khuyenmai in km.DefaultIfEmpty()
                                    where khoaHoc.TinhTrang == "daduyet"
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        Trinhdo = khoaHoc.TrinhDo,
                                        GiaKhoaHoc = khoaHoc.Gia,
                                        TenNguoiday = nguoiDay.TenNguoiDay,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        giadagiam = khuyenmai != null ? khuyenmai.GiaKhiGiam : null,
                                        phantramgiamgia = khuyenmai != null ? khuyenmai.PhanTramGiamGia : null,
                                    };

                var allProducts = _context.KhoaHocs.ToList();

                // Tính toán số lượng trang
                int totalProducts = allProducts.Count();
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                // Xác định sản phẩm cần hiển thị trên trang hiện tại
                var productsOnPage = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Tạo kết quả trả về
                var result = new
                {
                    //TotalPages = totalPages,
                    //CurrentPage = page,
                    KhoaHoc = productsOnPage
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpGet]
        [Route("GetKhoaHoctheodanhmuc")]
        public IActionResult GetKhoaHoctheodanhmuc(string danhmuc)
        {
            try
            {
                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into km
                                    from khuyenmai in km.DefaultIfEmpty()
                                    where khoaHoc.TinhTrang == "daduyet"
                                    where khoaHoc.DanhMuc == danhmuc
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        danhmuc = khoaHoc.DanhMuc,
                                        Trinhdo = khoaHoc.TrinhDo,
                                        GiaKhoaHoc = khoaHoc.Gia,
                                        TenNguoiday = nguoiDay.TenNguoiDay,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        giadagiam = khuyenmai != null ? khuyenmai.GiaKhiGiam : null,
                                        phantramgiamgia = khuyenmai != null ? khuyenmai.PhanTramGiamGia : null,
                                        //DiemNhanXet = NhanXet != null ? NhanXet.DiemNhanXet : 0,
                                    };

                // Tạo kết quả trả về
                var result = new
                {
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetKhoaHoctheoTrinhDo")]
        public IActionResult GetKhoaHoctheoTrinhDo(string trinhdo)
        {
            try
            {
                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into km
                                    from khuyenmai in km.DefaultIfEmpty()
                                    where khoaHoc.TinhTrang == "daduyet"
                                    where khoaHoc.TrinhDo == trinhdo
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        danhmuc = khoaHoc.DanhMuc,
                                        Trinhdo = khoaHoc.TrinhDo,
                                        GiaKhoaHoc = khoaHoc.Gia,
                                        TenNguoiday = nguoiDay.TenNguoiDay,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        giadagiam = khuyenmai != null ? khuyenmai.GiaKhiGiam : null,
                                        phantramgiamgia = khuyenmai != null ? khuyenmai.PhanTramGiamGia : null,
                                        //DiemNhanXet = NhanXet != null ? NhanXet.DiemNhanXet : 0,
                                    };

                // Tạo kết quả trả về
                var result = new
                {
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetKhoaHocTheoGia")]
        public IActionResult GetKhoaHoctheoGia(decimal giaTu, decimal giaDen)
        {
            try
            {
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into km
                                    from khuyenmai in km.DefaultIfEmpty()
                                    where khoaHoc.TinhTrang == "daduyet"
                                        && (khuyenmai != null ? khuyenmai.GiaKhiGiam : khoaHoc.Gia) >= giaTu
                                        && (khuyenmai != null ? khuyenmai.GiaKhiGiam : khoaHoc.Gia) <= giaDen
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        HinhAnh = khoaHoc.HinhAnh,
                                        danhmuc = khoaHoc.DanhMuc,
                                        Trinhdo = khoaHoc.TrinhDo,
                                        GiaKhoaHoc = khoaHoc.Gia,
                                        TenNguoiday = nguoiDay.TenNguoiDay,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        giadagiam = khuyenmai != null ? khuyenmai.GiaKhiGiam : null,
                                        phantramgiamgia = khuyenmai != null ? khuyenmai.PhanTramGiamGia : null,
                                    };

                var result = new
                {
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        [HttpGet]
        [Route("khoahocdangbanadmin")]
        public IActionResult Getkhoahocdangbanadmin()
        {
            try
            {
                //int pageSize = 10; // Số sản phẩm trên mỗi trang

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    where khoaHoc.TinhTrang == "daduyet"
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        danhmuc = khoaHoc.DanhMuc,
                                        danhmuccon = khoaHoc.DanhMucCon,
                                        tenNguoiday = nguoiDay.TenNguoiDay,
                                        giaban = khoaHoc.Gia,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        TinhTrang = khoaHoc.TinhTrang
                                    };


                //var allProducts = _context.KhoaHocs.ToList();

                //// Tính toán số lượng trang
                //int totalProducts = allProducts.Count();
                //int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                //// Xác định sản phẩm cần hiển thị trên trang hiện tại
                //var productsOnPage = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Tạo kết quả trả về
                var result = new
                {
                    //TotalPages = totalPages,
                    //CurrentPage = page,
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("Getdanhmuc")]
        public IActionResult Getdanhmuc()
        {
            try
            {
                // Lấy danh sách các tieude từ DanhMucKhoaHocs và chuyển thành mảng
                var danhMucTitles = _context.DanhMucKhoaHocs.Select(d => d.TieuDeDanhMuc).ToArray();

                return Ok(danhMucTitles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("Getkhoahocchuaduyetadmin")]
        public IActionResult Getkhoahocchuaduyetadmin()
        {
            try
            {
                //int pageSize = 10; // Số sản phẩm trên mỗi trang

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    where khoaHoc.TinhTrang == "chuaduyet"
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        danhmuc = khoaHoc.DanhMuc,
                                        danhmuccon = khoaHoc.DanhMucCon,
                                        tenNguoiday = nguoiDay.TenNguoiDay,
                                        giaban = khoaHoc.Gia,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        TinhTrang = khoaHoc.TinhTrang
                                    };


                //var allProducts = _context.KhoaHocs.ToList();

                //// Tính toán số lượng trang
                //int totalProducts = allProducts.Count();
                //int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                //// Xác định sản phẩm cần hiển thị trên trang hiện tại
                //var productsOnPage = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Tạo kết quả trả về
                var result = new
                {
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpGet]
        [Route("Getkhoahocbicam")]
        public IActionResult Getkhoahocbicam()
        {
            try
            {
                //int pageSize = 10; // Số sản phẩm trên mỗi trang

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    where khoaHoc.TinhTrang == "bicam"
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        danhmuc = khoaHoc.DanhMuc,
                                        danhmuccon = khoaHoc.DanhMucCon,
                                        tenNguoiday = nguoiDay.TenNguoiDay,
                                        phanhoi = khoaHoc.PhanHoi,
                                        giaban = khoaHoc.Gia,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        TinhTrang = "Bị cấm"
                                    };


                var result = new
                {
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("khoahocdangbannguoiday/{manguoiday}")]
        public IActionResult Getkhoahocdangbannguoiday(int manguoiday)
        {
            try
            {
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    join km in _context.KhuyenMaiCuaKhoaHocs on khoaHoc.MaKhoaHoc equals km.MaKhoaHoc into kmGroup
                                    from km in kmGroup.DefaultIfEmpty() // Sử dụng DefaultIfEmpty để giữ lại khóa học không có khuyến mãi
                                    where khoaHoc.TinhTrang == "daduyet"
                                    where khoaHoc.MaNguoiDay == manguoiday
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        danhmuc = khoaHoc.DanhMuc,
                                        danhmuccon = khoaHoc.DanhMucCon,
                                        tenNguoiday = nguoiDay.TenNguoiDay,
                                        giaban = khoaHoc.Gia,
                                        phantramgiam = km != null ? km.PhanTramGiamGia : 0,
                                        giagiam = km != null ? km.GiaKhiGiam : 0, // Lấy giảm giá nếu có, ngược lại giữ nguyên giá
                                        ngaybatdau = km != null && km.NgayBatDau != null ? km.NgayBatDau.Value.ToString("dd/MM/yyyy") : "0",
                                        ngayketthuc = km != null && km.NgayKetThuc != null ? km.NgayKetThuc.Value.ToString("dd/MM/yyyy") : "0",
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        TinhTrang = "đã duyệt",
                                    };

                var result = new
                {

                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("hoidapnguoiday/{manguoiday}")]
        public IActionResult hoidapnguoiday(int manguoiday)
        {
            try
            {
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay   
                                    join hd in _context.HoiDaps on khoaHoc.MaKhoaHoc equals hd.MaKhoaHoc
                                    where khoaHoc.TinhTrang == "daduyet"
                                    where khoaHoc.MaNguoiDay == manguoiday
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        soLuongCauHoi = _context.HoiDaps.Count(x => x.MaKhoaHoc == khoaHoc.MaKhoaHoc),
                                        macauhoi = hd.MaHoiDap,
                                    };

                var result = new
                {

                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("Getkhoahocchuaduyetnguoiday/{manguoiday}")]
        public IActionResult Getkhoahocchuaduyetnguoiday(int manguoiday)
        {
            try
            {
                //int pageSize = 10; // Số sản phẩm trên mỗi trang

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (database hoặc dữ liệu giả lập)
                var productsQuery = from khoaHoc in _context.KhoaHocs
                                    join nguoiDay in _context.NguoiDays on khoaHoc.MaNguoiDay equals nguoiDay.MaNguoiDay
                                    where khoaHoc.TinhTrang == "chuaduyet"
                                    where khoaHoc.MaNguoiDay == manguoiday
                                    select new
                                    {
                                        makhoahoc = khoaHoc.MaKhoaHoc,
                                        tieudekhoahoc = khoaHoc.TieuDeKhoaHoc,
                                        danhmuc = khoaHoc.DanhMuc,
                                        danhmuccon = khoaHoc.DanhMucCon,
                                        tenNguoiday = nguoiDay.TenNguoiDay,
                                        giaban = khoaHoc.Gia,
                                        SoLuongHocvien = khoaHoc.SoLuongHocVien,
                                        TinhTrang = "Chờ duyệt",
                                    };


                //var allProducts = _context.KhoaHocs.ToList();

                //// Tính toán số lượng trang
                //int totalProducts = allProducts.Count();
                //int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

                //// Xác định sản phẩm cần hiển thị trên trang hiện tại
                //var productsOnPage = productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Tạo kết quả trả về
                var result = new
                {
                    //TotalPages = totalPages,
                    //CurrentPage = page,
                    KhoaHoc = productsQuery
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public class PhanHoi
        {
            public string phanhoi { get; set; }
        }

        [HttpPut]
        [Route("Gobokhoahoc/{id}")]
        public IActionResult Gobokhoahoc(string id, PhanHoi ph)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourse = _context.KhoaHocs.Find(id);

            if (existingCourse == null)
            {
                return NotFound();
            }

            // Cập nhật thuộc tính của existingCourse với giá trị từ updatedCourse
            existingCourse.TinhTrang = "bicam";
            existingCourse.PhanHoi = ph.phanhoi;

            // Thực hiện lưu vào cơ sở dữ liệu
            _context.SaveChanges();

            return Ok(existingCourse);
        }


        public class GiamGiaDto
        {
            public float giamgia { get; set; }
            public string ghichu { get; set; }
        }

        [HttpPost]
        [Route("PostGiamGia/{manguoiday}")]
        public IActionResult PostGiamGia(int manguoiday, GiamGiaDto giamGiaDto)
        {
            try
            {
                // Lưu thông tin giảm giá vào cơ sở dữ liệu
                var giamGia = new KhuyenMai
                {
                    MaNguoiDay = manguoiday,
                    GiamGia = giamGiaDto.giamgia,
                    GhiChu = giamGiaDto.ghichu,
                    NgayTao = DateTime.Now,
                };

                _context.KhuyenMais.Add(giamGia);

                _context.SaveChangesAsync();

                return Ok("Thêm giảm giá thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetdanhsachGiamGia/{manguoiday}")]
        public IActionResult GetdanhsachGiamGia(int manguoiday)
        {
            try
            {
                // Lấy danh sách giảm giá của người dạy có mã là manguoiday
                var danhSachGiamGia = _context.KhuyenMais
                    .Where(khuyenMai => khuyenMai.MaNguoiDay == manguoiday)
                    .Select(khuyenMai => new
                    {
                        khuyenMai.MaGiamGia,
                        khuyenMai.GiamGia,
                        khuyenMai.GhiChu,
                        khuyenMai.NgayTao
                    })
                    .ToList();

                return Ok(danhSachGiamGia);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpDelete]
        [Route("DeleteGiamGia/{manguoiday}/{discountId}")]
        public IActionResult DeleteGiamGia(int manguoiday, int discountId)
        {
            try
            {
                // Find the discount by discountId and teacherId
                var discount = _context.KhuyenMais
                    .FirstOrDefault(khuyenMai => khuyenMai.MaGiamGia == discountId && khuyenMai.MaNguoiDay == manguoiday);

                if (discount == null)
                {
                    // If discount is not found, return not found response
                    return NotFound($"Discount with ID {discountId} for teacher ID {manguoiday} not found.");
                }

                // Remove the discount from the context and save changes
                _context.KhuyenMais.Remove(discount);
                _context.SaveChanges();

                return Ok("Xóa giảm giá thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public class GiamGia
        {
         
            public string MaKhoaHoc { get; set; }
            public int PhanTramGiamGia { get; set; }
            public int GiaApDungMa { get; set; }
            public int GiaSauKhiGiam { get; set; }
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
        }


        [HttpPost]
        [Route("Postapdungma/{makhoahoc}")]
        public IActionResult Postapdungma(string makhoahoc, GiamGia gg)
        {
            try
            { // Kiểm tra xem khóa học có tồn tại không
                var khoaHoc = _context.KhoaHocs.FirstOrDefault(kh => kh.MaKhoaHoc == makhoahoc);
                if (khoaHoc == null)
                {
                    return NotFound($"Khóa học với mã {makhoahoc} không tồn tại.");
                }

                // Kiểm tra xem mã giảm giá đã tồn tại chưa
                var existingGiamGia = _context.KhuyenMaiCuaKhoaHocs.FirstOrDefault(gg => gg.MaKhoaHoc == makhoahoc && gg.GiaApDungMa == gg.GiaApDungMa);
                if (existingGiamGia != null)
                {
                    return Conflict($"Mã giảm giá {gg.GiaApDungMa} đã tồn tại cho khóa học có mã {makhoahoc}.");
                }

                var giamgiakhoahoc = new KhuyenMaiCuaKhoaHoc
                {
                    MaKhoaHoc = makhoahoc,
                    PhanTramGiamGia = gg.PhanTramGiamGia,
                    GiaApDungMa = gg.GiaApDungMa,
                    GiaKhiGiam = gg.GiaSauKhiGiam,
                    NgayBatDau = gg.NgayBatDau,
                    NgayKetThuc = gg.NgayKetThuc,
                };


                // Thêm vào cơ sở dữ liệu
                _context.KhuyenMaiCuaKhoaHocs.Add(giamgiakhoahoc);
                _context.SaveChanges();

                return Ok($"Đã áp dụng thành công mã giảm giá");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // HttpGet: api/RequestToHired/getFileRequestToHired
        //[HttpGet]
        //[Route("getFileRequestToHired")]
        //public IActionResult GetFileRequestToHired(string fileNameId)
        //{
        //    var temp = fileNameId.Split('.');
        //    var fileBytes = System.IO.File.ReadAllBytes(string.Concat("C:\\Users\\pc\\Desktop\\doantotnghiep\\DoAn\\BackEnd\\sever\\Learning-hub\\wwwroot\\images", "\\", fileNameId));

        //    switch (temp[1])
        //    {
        //        case "jpg":
        //        case "png":
        //            return File(fileBytes, "image/jpeg");
        //        case "docx":
        //            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        //        case "pdf":
        //            return File(fileBytes, "application/pdf");
        //        default:
        //            return Ok();
        //    }
        //}


    }
}
