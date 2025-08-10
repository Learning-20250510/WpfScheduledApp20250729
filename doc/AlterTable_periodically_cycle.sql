-- ========================================
-- periodically_cycle テーブル制約・初期値設定
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1. デフォルト値設定
-- 1-1. created_at のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN created_at SET DEFAULT NOW();

-- 1-2. updated_at のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN updated_at SET DEFAULT NOW();

-- 1-3. touched_at のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN touched_at SET DEFAULT NOW();

-- 1-4. last_touched_method_name のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN last_touched_method_name SET DEFAULT null;

-- 1-5. error_message のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN error_message SET DEFAULT null;

-- 1-6. archived のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN archived SET DEFAULT false;

-- 1-7. disabled のデフォルト値設定
ALTER TABLE public.periodically_cycle 
ALTER COLUMN disabled SET DEFAULT false;

-- 2. 更新トリガー関数の作成（periodically_cycle用）
CREATE OR REPLACE FUNCTION update_periodically_cycle_timestamps()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = NOW();
    NEW.touched_at = NOW();
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- 3-1. 既存のトリガーを削除（存在する場合）
DROP TRIGGER IF EXISTS trigger_update_periodically_cycle_timestamps ON public.periodically_cycle;

-- 3-2. 更新トリガーの作成
CREATE TRIGGER trigger_update_periodically_cycle_timestamps
    BEFORE UPDATE ON public.periodically_cycle
    FOR EACH ROW
    EXECUTE FUNCTION update_periodically_cycle_timestamps();

-- 4. cycle カラムの制約
-- 4-1. cycle 空文字禁止制約
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT check_cycle_not_empty 
CHECK (char_length(trim(cycle)) > 0);

-- 4-2. cycle 一意制約（重複不可）
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT unique_cycle 
UNIQUE (cycle);

-- 5. タイムスタンプ制約
-- 5-1. created_at 未来日付禁止制約
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT check_created_at_not_future 
CHECK (created_at <= NOW());

-- 5-2. updated_at 未来日付禁止制約
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT check_updated_at_not_future 
CHECK (updated_at <= NOW());

-- 5-3. touched_at 未来日付禁止制約
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT check_touched_at_not_future 
CHECK (touched_at <= NOW());

-- 5-4. updated_at が created_at より前でない制約
ALTER TABLE public.periodically_cycle 
ADD CONSTRAINT check_updated_at_after_created_at 
CHECK (updated_at >= created_at);

-- 6. パフォーマンス向上用インデックス
-- 6-1. cycle インデックス（一意制約で自動作成されるが明示的に作成）
CREATE INDEX IF NOT EXISTS idx_periodically_cycle_cycle 
ON public.periodically_cycle(cycle);

-- 6-2. created_at インデックス
CREATE INDEX IF NOT EXISTS idx_periodically_cycle_created_at 
ON public.periodically_cycle(created_at);

-- 6-3. updated_at インデックス
CREATE INDEX IF NOT EXISTS idx_periodically_cycle_updated_at 
ON public.periodically_cycle(updated_at);

-- 6-4. archived, disabled 複合インデックス
CREATE INDEX IF NOT EXISTS idx_periodically_cycle_archived_disabled 
ON public.periodically_cycle(archived, disabled) 
WHERE archived = false AND disabled = false;