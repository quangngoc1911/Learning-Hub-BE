using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("Course")]
    public class DataCourse
    {
        [Key]
        public string IdKhoaHoc { get; set; }
        [ForeignKey("UserModel")]
        public int IdNguoiDay { get; set; }
        public DataTeacher UserModel { get; set; }
        public string TieuDeKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public string KienThucThuDuoc { get; set; }
        public string TrinhDo { get; set; }
        public string DanhMuc { get; set; }
        public string DanhMucCon { get; set; }
        public string HinhAnh { get; set; }
        public int Gia { get; set; }
        public int SoLuongHocVien { get; set; }


    }
}
