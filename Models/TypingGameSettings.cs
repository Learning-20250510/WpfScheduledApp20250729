using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Models
{
    [Table("typing_game_settings")]
    public class TypingGameSettings : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("architecture_id")]
        public int? ArchitectureId { get; set; }
        
        [Column("how_to_learn_id")]
        public int? HowToLearnId { get; set; }
        
        [Column("max_progress_value")]
        public int MaxProgressValue { get; set; } = 1000;
        
        [Column("decrement_per_key")]
        public int DecrementPerKey { get; set; } = 5;
        
        [Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;
        
        [Column("progress_bar_color")]
        [MaxLength(20)]
        public string ProgressBarColor { get; set; } = "#FF00FFFF";
        
        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        public TypingGameSettings()
        {
            Id = 0;
        }
    }
}