using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfScheduledApp20250629.Models.Entities
{
    [Table("last_upd_pgm")]
    internal class LastUpdPgm : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id")]
        public int Id { get; set; }
        [Column("pgm_name")]
        [Required]
        [MaxLength(100)]
        public string PgmName { get; set; } = string.Empty;


    }
}
