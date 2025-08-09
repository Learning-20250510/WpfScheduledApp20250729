using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("periodically_cycle")]
    public class PeriodicallyCycle : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("cycle")]
        [Required]
        [MaxLength(100)]
        public string Cycle { get; set; } = string.Empty;
    }
}
