using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("middle_task")]
    public class MiddleTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        // 外部キー
        [Column("high_task_id")]
        public int HighTaskId { get; set; }

        [Column("htl_id")]
        public int HtlId { get; set; }


        [Column("project_id")]
        public int ProjectId { get; set; }

        [Column("description")]
        [MaxLength(1000)]
        public string? Description { get; set; }


        [Column("can_auto_reschedule")]
        public bool CanAutoReschedule { get; set; }

        [Column("estimated_time")]
        public int EstimatedTime { get; set; }

        [Column("lastcleared_at", TypeName = "timestamp with time zone")]
        public DateTimeOffset? LastClearedAt { get; set; }

        [Column("clear_times_intime")]
        public int ClearTimesInTime { get; set; }

        [Column("clear_times_outoftime")]
        public int ClearTimesOutofTime { get; set; }

        [Column("periodically_cycles_id")]
        public int PeriodicallyCyclesId { get; set; }

        [Column("file_name")]
        [MaxLength(255)]
        public string? FileName { get; set; }

        [Column("url")]
        [MaxLength(500)]
        public string? Url { get; set; }

        [Column("specified_page_as_pdf")]
        public int SpecifiedPageAsPdf { get; set; }

        [Column("specified_scroll_amount_as_url")]
        public int SpecifiedScrollAmountAsUrl { get; set; }

        // ビュー専用プロパティは NotMapped のまま
        [NotMapped]
        public bool CheckBox { get; set; }
        [NotMapped]
        public string HighTaskName { get; set; } = string.Empty;
        [NotMapped]
        public string HowToLearnName { get; set; } = string.Empty;
        [NotMapped]
        public string ProjectName { get; set; } = string.Empty;
        [NotMapped]
        public string ArchitectureName { get; set; } = string.Empty;
        [NotMapped]
        public string PeriodicallyCyclesName { get; set; } = string.Empty;
    }
}
