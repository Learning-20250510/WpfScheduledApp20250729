using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Models.Entities
{

    [Table("relation_extension_app")]
    public class RelationExtensionApp : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("extension")]
        [Required]
        [MaxLength(200)]
        public string Extension { get; set; } = string.Empty;

        [Column("application")]
        [Required]
        [MaxLength(200)]
        public string Application { get; set; } = string.Empty;
    }
}
