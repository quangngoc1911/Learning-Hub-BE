using System;
using System.Collections.Generic;

namespace Learning_hub.Models
{
    public class KhoaHoc
    {
        public class CourseData
        {
            public List<Chuong> formattedData { get; set; }
            public string MaKhoaHoc { get; set; }
        }
        public class Chuong
        {

            public string Title { get; set; }
            public List<BaiGiang> Lessons { get; set; }
        }
        public class BaiGiang
        {
            public int thoiluong { get; set; }
            public string File { get; set; }
            public string title { get; set; }
            public string Video { get; set; }
            public bool xemtruoc { get; set; } 
        }
        public class QuestionModel
        {
            public string TieuDeCauHoi { get; set; }
            public List<AnswerModel> DapAns { get; set; }
            public string DapAnDung { get; set; }
        }

        public class AnswerModel
        {
            public string TieuDeDapAn { get; set; }
        }

        public class CauHoiRequest
        {
            public string maKhoaHoc { get; set; }
            public List<QuestionModel> questions { get; set; }
        }
        public class GiaKhoaHoc
        {
            public string maKhoaHoc { get; set; }
            public int giakhoahoc { get; set; }
        }


        public class KhoaHocMoi
        {
            public string MaKhoaHoc { get; set; }
            public string TieuDeKhoaHoc { get; set; }
            public string MoTa { get; set; }
            public string KienThucThuDuoc { get; set; }
            public string TrinhDo { get; set; }
            public string DanhMuc { get; set; }
            public string DanhMucCon { get; set; }
            public string HinhAnh { get; set; }
            public int? Gia { get; set; }
            public int? SoLuongHocVien { get; set; }
            public DateTime? NgayTao { get; set; }
        }






    }
}
