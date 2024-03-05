using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Learning_hub.Data;
using Learning_hub.Entities;
using Learning_hub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Learning_hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly LearningHubContext _context;
        private readonly AppSetting _appSettings;

        public class LinhVucDay
        {
            
            public int MaNguoiDay { get; set; }
            public int MaDanhMuc { get; set; }
        }

            public ProfileController(LearningHubContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }


        [HttpPut]
        [Route("CapNhat-NguoiDay/{id}")]
        public async Task<IActionResult> PutNguoiDay([FromForm] CapNhatNguoiDay model, int id)
        {


            Account account = new Account("ddof16h09", "975568459733683", "wDD3JgPxewySR6tAnswwU3aqjfQ");
            Cloudinary cloudinary = new Cloudinary(account);

            if (model.hinhanh != null && model.hinhanh.Length > 0)
            {
                try
                {

                    var result = cloudinary.UploadLarge(new ImageUploadParams
                    {
                        File = new FileDescription(model.hinhanh.FileName, model.hinhanh.OpenReadStream()),
                        PublicId = "test"
                    });
                    var imageUrl = result.Url.ToString(); ; // Đường dẫn ảnh trên Cloudinary


                    var nguoiDay = _context.NguoiDays.FirstOrDefault(e => e.MaNguoiDay == id);
                    if (nguoiDay != null)
                    {
                        nguoiDay.HinhAnh = imageUrl;
                        nguoiDay.GioiThieuBanThan = model.gioithieubanthan;
                        nguoiDay.TinhTrang = "chuaduyet";
                        nguoiDay.Cccd = model.cccd;
                        await _context.SaveChangesAsync();

                        return Ok(new { message = "Gửi yêu cầu phê duyệt thành công" });
                    }

                }
                catch (Exception e)
                {
                    return Ok(e);
                }

                return BadRequest();
                    
                }
            return BadRequest();
        }



        [HttpPost]
        [Route("LinhVuc-NguoiDay")]
        public async Task<IActionResult> PostLinhVuc(List<LinhVucDay> models)
        {
            if (models != null && models.Any())
            {
                // Tạo danh sách mới để lưu thông tin
                var newLinhVucs = models.Select(model => new LinhVucGiangDay
                {
                    MaNguoiDay = model.MaNguoiDay,
                    MaDanhmuc = model.MaDanhMuc,
                }).ToList();

                // Thêm danh sách mới vào cơ sở dữ liệu
                _context.LinhVucGiangDays.AddRange(newLinhVucs);
                await _context.SaveChangesAsync();
            }
            return Ok("lĩnh vực được thêm thành công");
        }






        }
}
