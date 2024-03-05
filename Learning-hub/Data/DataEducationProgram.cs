using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("EducationProgram")]
    public class DataEducationProgram
    {
        [Key]
        public int IdChuongTrinh { get; set; }
        public int IdNguoiDay { get; set; }
        public string TieuDeMuc { get; set; }
    }
}
