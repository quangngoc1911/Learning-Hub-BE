using Learning_hub.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Linq;
using Learning_hub.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using static Learning_hub.Controllers.ProfileController;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Learning_hub.Api;
using Learning_hub.Entities;
using static Learning_hub.Models.KhoaHoc;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly MyDbContext _contexts;
        private readonly LearningHubContext _contexts;
        private readonly AppSetting _appSettings;
        private readonly IConfiguration _configuration;

        public UserController(LearningHubContext ctx, IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration)
        {
            _contexts = ctx;
            _appSettings = optionsMonitor.CurrentValue;
            _configuration = configuration;
        }
       

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Vui lòng nhập tên đăng nhập và mật khẩu"
                });
            }

            var nguoiday = _contexts.NguoiDays.SingleOrDefault(p => p.TenDangNhap == model.Username && model.Password == p.MatKhau);
            var hocvien = _contexts.HocViens.SingleOrDefault(p => p.TenDangNhap == model.Username && model.Password == p.MatKhau);

            if (nguoiday == null && hocvien == null)
            {
                return Unauthorized("Tài khoản không tồn tại");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            if (nguoiday != null)
            {
                if(nguoiday.Role == "nguoiday")
                {
                    return Ok(new ApiResponse
                    {
                        id = nguoiday.MaNguoiDay,
                        Success = true,
                        Message = "Đăng nhập người dạy thành công",
                        Role = nguoiday.Role,
                        TinhTrang = nguoiday.TinhTrang,
                    });
                }
                else if (nguoiday.Role == "admin")
                {
                    return Ok(new ApiResponse
                    {
                        id = nguoiday.MaNguoiDay,
                        Success = true,
                        Message = "Đăng nhập admin thành công",
                        Role = nguoiday.Role,
                    });
                }

            }
            else if (hocvien != null)
            {
                return Ok(new ApiResponse
                {
                    id = hocvien.MaHocVien,
                    Success = true,
                    Message = "Đăng nhập người học thành công",
                    Role = hocvien.Role,
                });
            }
            return Unauthorized("tài khoản không tồn tại");

        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetProfileAsync(int id)
        {
            var nguoiday = await _contexts.NguoiDays.Where(u => u.MaNguoiDay == id)
               .Select(u => new 
               {
                   Email = u.Email,
                   FullName = u.TenNguoiDay,
               })
               .FirstOrDefaultAsync();
            

            if (nguoiday == null)
            {
                var hocvien = await _contexts.HocViens.Where(u => u.MaHocVien == id)
                   .Select(u => new
                   {
                       Email = u.Email,
                       FullName = u.TenHocVien,
                   })
                   .FirstOrDefaultAsync();
                if (hocvien == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(hocvien);
                }
            }
            else { 
                return Ok(nguoiday); 
            }
        }
        private async Task<int> GenerateUniqueTeacherCode()
        {
            Random random = new Random();
            int randomNumber;
            NguoiDay existingStudent;

            do
            {
                randomNumber = random.Next(100, 1000);
                existingStudent = await _contexts.NguoiDays.FirstOrDefaultAsync(hv => hv.MaNguoiDay == randomNumber);
            } while (existingStudent != null);

            return randomNumber;
        }

        [HttpPost("DangKy_DayHoc")]
        public async Task<IActionResult> DayHoc(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Vui lòng điền đầy đủ thông tin" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu chưa
            var existingUser = await _contexts.NguoiDays.FirstOrDefaultAsync(u => u.TenDangNhap == model.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email đã được đăng ký" });
            }

            
            // Tạo một đối tượng User mới và thêm vào cơ sở dữ liệu
            var newUser = new NguoiDay
            {
                MaNguoiDay = await GenerateUniqueTeacherCode(),
                TenNguoiDay = model.FullName,
                Email = model.Email,
                Role = "nguoiday",
                TinhTrang = "chuaduyet",
                NgayTao = DateTime.Now,
                TenDangNhap = model.UserName,
                MatKhau = model.Password,
            };

             _contexts.NguoiDays.Add(newUser);
             await _contexts.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }



        private async Task<int> GenerateUniqueStudentCode()
        {
            Random random = new Random();
            int randomNumber;
            HocVien existingStudent;

            do
            {
                randomNumber = random.Next(1000, 10000);
                existingStudent = await _contexts.HocViens.FirstOrDefaultAsync(hv => hv.MaHocVien == randomNumber);
            } while (existingStudent != null);

            return randomNumber;
        }

        [HttpPost("DangKy_HocVien")]
        public async Task<IActionResult> HocVien(RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu chưa
            var existingUser = await _contexts.HocViens.FirstOrDefaultAsync(u => u.TenDangNhap == model.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "tên đăng nhập đã được đăng ký" });
            }

    
            // Tạo một đối tượng User mới và thêm vào cơ sở dữ liệu
            var newUser = new HocVien
            {
                MaHocVien = await  GenerateUniqueStudentCode(),
                TenHocVien = model.FullName,
                Email = model.Email,
                NgayTao = DateTime.Now,
                Role = "hocvien",
                TenDangNhap = model.UserName,
                MatKhau = model.Password,
            };

            _contexts.HocViens.Add(newUser);
            await _contexts.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }
       
    }
}
