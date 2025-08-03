using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfScheduledApp20250729.Models.Entities
{
    internal abstract class BaseEntity
    {
        // INSERT 時に DB 側で自動設定
        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; private set; }

        // INSERT/UPDATE 時に DB 側で自動更新
        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; private set; }

        // 任意のタイミングでアプリ側から明示更新
        [Column("touched_at")]
        public DateTime TouchedAt { get; set; }

        // アプリ側で都度セット（既定値不要）
        [Column("last_upd_pgm_at")]
        public int LastUpdPgmId { get; set; }

        [Column("last_upd_user")]
        [MaxLength(100)]
        public string LastUpdUser { get; set; } = string.Empty;

        [Column("last_upd_method_name")]
        [MaxLength(200)]
        public string LastUpdMethodName { get; set; } = string.Empty;

        [Column("error_message")]
        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        // デフォルト FALSE を DB で設定
        [Column("archived")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Archived { get; private set; } = false;

        [Column("disabled")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Disabled { get; private set; } = false;
    }
}
