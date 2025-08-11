using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("motivation")]
    public class Motivation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("motivation_name")]
        [Required]
        [MaxLength(100)]
        public string MotivationName { get; set; } = string.Empty;

        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Column("message")]
        [MaxLength(1000)]
        public string? Message { get; set; }

        [Column("icon")]
        [MaxLength(50)]
        public string? Icon { get; set; }

        [Column("color")]
        [MaxLength(20)]
        public string? Color { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; } = 0;
    }
}