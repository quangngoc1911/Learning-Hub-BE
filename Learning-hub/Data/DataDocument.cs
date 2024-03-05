using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("Document")]
    public class DataDocument
    {
        [Key]
        public int IdTaiLieu { get; set; }
        public int IdBaiGiang { get; set; }
        public string TenTaiLieu { get; set; }
        public string LoaiTaiLieu { get; set; }
        public int DuongDan { get; set; }
    }
}
