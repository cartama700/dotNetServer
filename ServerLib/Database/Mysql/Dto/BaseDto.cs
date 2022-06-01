using System.ComponentModel.DataAnnotations;

namespace ServerLib.Database.Mysql.Dto
{
    /// <summary>
    /// Dto에 들어가는 기본 정보
    /// </summary>
    public record BaseDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:i:s}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:i:s}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
