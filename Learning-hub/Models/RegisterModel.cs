using System;
using System.ComponentModel.DataAnnotations;

namespace Learning_hub.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        
    }
}
