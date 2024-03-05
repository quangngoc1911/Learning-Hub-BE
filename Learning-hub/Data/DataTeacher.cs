using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("Teacher")]
    public class DataTeacher
    {
        [Key]
        [ForeignKey("UserModel")]
        public string IdNguoiDay { get; set; }
        public DataUser UserModel { get; set; }
        public string TenNguoiDay { get; set; }
        public string Email { get; set; }
        public string GioiThieuBanThan { get; set; }
        public string LinhVucDay { get; set; }
        public string HinhAnh { get; set; }
        public int TinhTrang { get; set; }
    }
}
