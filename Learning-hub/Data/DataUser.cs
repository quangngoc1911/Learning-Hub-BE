using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Data
{
    [Table("User")]
    public class DataUser
    {
        [Key]
        public string IdUser { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string PassWord { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int CCCD { get; set; }

        [MaxLength(5)]
        public int Status { get; set; }

        [MaxLength(10)]
        public int Role { get; set; }

    }
}
