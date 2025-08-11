-- Motivationテーブル作成
CREATE TABLE IF NOT EXISTS motivation (
    id SERIAL PRIMARY KEY,
    motivation_name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(500),
    message VARCHAR(1000),
    icon VARCHAR(50),
    color VARCHAR(20),
    display_order INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    touched_at TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    archived BOOLEAN NOT NULL DEFAULT false,
    disabled BOOLEAN NOT NULL DEFAULT false,
    last_upd_method_name VARCHAR(200)
);

-- インデックス作成
CREATE INDEX IF NOT EXISTS idx_motivation_name ON motivation(motivation_name);
CREATE INDEX IF NOT EXISTS idx_motivation_display_order ON motivation(display_order);

-- コメント追加
COMMENT ON TABLE motivation IS 'モチベーション管理テーブル';
COMMENT ON COLUMN motivation.id IS 'プライマリキー';
COMMENT ON COLUMN motivation.motivation_name IS 'モチベーション名（一意制約）';
COMMENT ON COLUMN motivation.description IS 'モチベーションの説明';
COMMENT ON COLUMN motivation.message IS 'ユーザーへ表示するメッセージ';
COMMENT ON COLUMN motivation.icon IS '表示アイコン（絵文字など）';
COMMENT ON COLUMN motivation.color IS '表示色（16進数）';
COMMENT ON COLUMN motivation.display_order IS '表示順序';
COMMENT ON COLUMN motivation.created_at IS '作成日時';
COMMENT ON COLUMN motivation.updated_at IS '更新日時';
COMMENT ON COLUMN motivation.touched_at IS '最終アクセス日時';
COMMENT ON COLUMN motivation.archived IS 'アーカイブフラグ';
COMMENT ON COLUMN motivation.disabled IS '無効フラグ';
COMMENT ON COLUMN motivation.last_upd_method_name IS '最終更新メソッド名';