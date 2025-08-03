using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250629.Models.Entities
{
    [Table("project")]
    internal class Project : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; private set; }

        [Column("project_name")]
        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; } = string.Empty;
    }
}
