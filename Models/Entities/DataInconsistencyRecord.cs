using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("data_inconsistency_record")]
    public class DataInconsistencyRecord : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("rule_name")]
        [Required]
        [MaxLength(100)]
        public string RuleName { get; set; } = string.Empty;

        [Column("error_description")]
        [Required]
        [MaxLength(500)]
        public string ErrorDescription { get; set; } = string.Empty;

        [Column("target_table_name")]
        [Required]
        [MaxLength(100)]
        public string TargetTableName { get; set; } = string.Empty;

        [Column("target_primary_key")]
        [Required]
        [MaxLength(50)]
        public string TargetPrimaryKey { get; set; } = string.Empty;

        [Column("severity")]
        [Required]
        public string Severity { get; set; } = string.Empty;

        [Column("details")]
        [MaxLength(2000)]
        public string? Details { get; set; }

        [Column("detected_at", TypeName = "timestamp without time zone")]
        public DateTime DetectedAt { get; set; }

        [Column("is_resolved")]
        public bool IsResolved { get; set; } = false;

        [Column("resolved_at", TypeName = "timestamp without time zone")]
        public DateTime? ResolvedAt { get; set; }

        [Column("resolved_by")]
        [MaxLength(100)]
        public string? ResolvedBy { get; set; }

        [Column("resolution_notes")]
        [MaxLength(1000)]
        public string? ResolutionNotes { get; set; }
    }
}