using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Models.Entities
{
    [Table("low_task")]
    public class LowTask : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id")]
        public int Id { get; set; }
        [Column("middle_task_id")]
        [Required]
        [ForeignKey(nameof(MiddleTask))]
        public int MiddleTaskId { get; set; }
        [Column("project_id")]
        [Required]
        public int ProjectId { get; set; }
        [Column("estimated_time")]
        public int EstimatedTime { get; set; }

        [Column("description")]
        public required string Description { get; set; }
        [Column("execution_date")]
        public DateOnly ExecutionDate { get; set; }
        [Column("execution_time")]
        public TimeOnly ExecutionTime { get; set; }
        [Column("can_auto_reschedule")]
        public bool CanAutoReschedule { get; set; }
        [Column("lastcleared_at")]
        public DateTime LastClearedAt { get; set; }
        [Column("clear_times_intime")]
        public int ClearTimesInTime { get; set; }
        [Column("clear_times_outoftime")]
        public int ClearTimesOutofTime { get; set; }

        [NotMapped]
        public bool CheckBox { get; set; }
        [NotMapped]
        public required string MiddleTaskMName { get; set; }
        [NotMapped]
        public  required string HowToLearnName { get; set; }
        [NotMapped]
        public required string ProjectName { get; set; }
    }
}
