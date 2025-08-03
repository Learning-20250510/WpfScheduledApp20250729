using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250629.Models.Entities
{
    [Table("architecture")]
    internal class Architecture : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("architecture_name")]
        [Required]
        [MaxLength(200)]
        public string ArchitectureName { get; set; } = string.Empty;
    }
}
