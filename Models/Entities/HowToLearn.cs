using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("how_to_learn")]
    public class HowToLearn : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("htl")]
        [Required]
        [MaxLength(200)]
        public string Htl { get; set; } = string.Empty;
    }
}
