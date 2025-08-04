-- ========================================
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1-1. created_at のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN created_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-2. updated_at のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN updated_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-3. touched_at のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN touched_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-4. last_touched_method_name のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN last_touched_method_name SET DEFAULT null;

-- 1-5. error_message のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN error_message SET DEFAULT null;

-- 1-6. archived のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN archived SET DEFAULT false;

-- 1-7. disabled のデフォルト値設定
ALTER TABLE public.architecture 
ALTER COLUMN disabled SET DEFAULT false;

-- 2. 更新トリガー関数の作成
CREATE OR REPLACE FUNCTION update_architecture_timestamps()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    NEW.touched_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- 3-1. 既存のトリガーを削除（存在する場合）
DROP TRIGGER IF EXISTS trigger_update_architecture_timestamps ON public.architecture;

-- 3-2. 更新トリガーの作成
CREATE TRIGGER trigger_update_architecture_timestamps
    BEFORE UPDATE ON public.architecture
    FOR EACH ROW
    EXECUTE FUNCTION update_architecture_timestamps();

-- 4-1. architecture_name 空文字禁止制約
ALTER TABLE public.architecture 
ADD CONSTRAINT check_architecture_name_not_empty 
CHECK (char_length(trim(architecture_name)) > 0);

-- 4-2. architecture_name 一意制約（重複不可）
ALTER TABLE public.architecture 
ADD CONSTRAINT unique_architecture_name 
UNIQUE (architecture_name);

-- 5-1. created_at 未来日付禁止制約
ALTER TABLE public.architecture 
ADD CONSTRAINT check_created_at_not_future 
CHECK (created_at <= CURRENT_TIMESTAMP);

-- 5-2. updated_at 未来日付禁止制約
ALTER TABLE public.architecture 
ADD CONSTRAINT check_updated_at_not_future 
CHECK (updated_at <= CURRENT_TIMESTAMP);

-- 5-3. touched_at 未来日付禁止制約
ALTER TABLE public.architecture 
ADD CONSTRAINT check_touched_at_not_future 
CHECK (touched_at <= CURRENT_TIMESTAMP);

-- 5-4. updated_at が created_at より前でない制約
ALTER TABLE public.architecture 
ADD CONSTRAINT check_updated_at_after_created_at 
CHECK (updated_at >= created_at);

-- 6-1. created_at インデックス
CREATE INDEX IF NOT EXISTS idx_architecture_created_at 
ON public.architecture(created_at);

-- 6-2. updated_at インデックス
CREATE INDEX IF NOT EXISTS idx_architecture_updated_at 
ON public.architecture(updated_at);

-- 6-3. archived, disabled 複合インデックス
CREATE INDEX IF NOT EXISTS idx_architecture_archived_disabled 
ON public.architecture(archived, disabled) 
WHERE archived = false AND disabled = false;