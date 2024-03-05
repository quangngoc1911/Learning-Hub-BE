using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("RequestApproval")]
    public class DataRequestApproval
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Describe { get; set; }
        public string FileName { get; set; }
        public int Status { get; set;}
        public string TeachingFields { get; set; }
    }
}
