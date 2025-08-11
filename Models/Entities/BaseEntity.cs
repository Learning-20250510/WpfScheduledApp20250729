using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    public abstract class BaseEntity
    {
        // INSERT 時にアプリ側で設定
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // UPDATE 時にアプリ側で設定
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // 任意のタイミングでアプリ側から明示更新
        [Column("touched_at")]
        public DateTime TouchedAt { get; set; }

        [Column("last_touched_method_name")]
        [MaxLength(200)]
        public string LastUpdMethodName { get; set; } = string.Empty;

        [Column("error_message")]
        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        // デフォルト FALSE を設定
        [Column("archived")]
        public bool Archived { get; set; } = false;

        [Column("disabled")]
        public bool Disabled { get; set; } = false;
    }
}
