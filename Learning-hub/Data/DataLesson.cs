using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("Lesson")]
    public class DataLesson
    {
        [Key]
        public int IdBaiGiang { get; set; }
        public int IdKhoaHoc { get; set; }
        public int TieuDeBaiGiang { get; set; }
    }
}
