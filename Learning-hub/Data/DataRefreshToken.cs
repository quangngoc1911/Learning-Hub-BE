using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace Learning_hub.Data
{
    [Table("RefreshToken")]
    public class DataRefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(IdUser))]
        public string IdUser { get; set; }
        public DataUser UserModel { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
