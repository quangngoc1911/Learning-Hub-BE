using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_hub.Models
{

    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
