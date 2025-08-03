using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250629.Models.Entities
{
    [Table("task_specified_date")]
    internal class TaskSpecifiedDate : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Column("specified_date", TypeName = "date")]
        public DateOnly SpecifiedDate { get; set; }
    }
}
