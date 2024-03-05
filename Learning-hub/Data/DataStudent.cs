using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("Student")]
    public class DataStudent
    {
        public string IdNguoiHoc { get; set; }
        public string TenNguoiHoc { get; set; }
    }
}
