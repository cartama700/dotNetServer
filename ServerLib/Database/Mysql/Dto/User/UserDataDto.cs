using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerLib.Database.Mysql.Dto.User
{
    [Table("user_data")]
    [Comment("유저의 기본 데이터")]
    [Index(nameof(PlayerName), IsUnique = true, Name = "UNQ_PlayerName")]
    public record UserDataDto : BaseDto, IUserBase
    {
        [Key, Column(Order = 0)]
        [Comment("플레이어 아이디")]
        public long PlayerId { get; set; }

        [Column(Order = 1)]
        [Required]
        [MaxLength(12)]
        public string PlayerName { get; set; }
    }
}
