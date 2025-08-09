using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("high_task")]
    public class HighTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("architecture_id")]
        [ForeignKey(nameof(Architecture))]
        public int ArchitectureId { get; set; }
        [Column("taskname")]
        [Required]
        public required string TaskName { get; set; } = string.Empty;
        [Column("description")]
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("clear_times_intime")]
        public int ClearTimesInTime { get; set; }
        [Column("clear_times_outoftime")]
        public int ClearTimesOutofTime { get; set; }
    }
}
