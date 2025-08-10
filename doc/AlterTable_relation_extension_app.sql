-- ========================================
-- relation_extension_app テーブル制約・初期値設定
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1. デフォルト値設定
-- 1-1. created_at のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN created_at SET DEFAULT NOW();

-- 1-2. updated_at のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN updated_at SET DEFAULT NOW();

-- 1-3. touched_at のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN touched_at SET DEFAULT NOW();

-- 1-4. last_touched_method_name のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN last_touched_method_name SET DEFAULT null;

-- 1-5. error_message のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN error_message SET DEFAULT null;

-- 1-6. archived のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN archived SET DEFAULT false;

-- 1-7. disabled のデフォルト値設定
ALTER TABLE public.relation_extension_app 
ALTER COLUMN disabled SET DEFAULT false;

-- 2. 更新トリガー関数の作成（relation_extension_app用）
CREATE OR REPLACE FUNCTION update_relation_extension_app_timestamps()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = NOW();
    NEW.touched_at = NOW();
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- 3-1. 既存のトリガーを削除（存在する場合）
DROP TRIGGER IF EXISTS trigger_update_relation_extension_app_timestamps ON public.relation_extension_app;

-- 3-2. 更新トリガーの作成
CREATE TRIGGER trigger_update_relation_extension_app_timestamps
    BEFORE UPDATE ON public.relation_extension_app
    FOR EACH ROW
    EXECUTE FUNCTION update_relation_extension_app_timestamps();

-- 4. extension と application の制約
-- 4-1. extension は空文字OK（制約なし）
-- 空文字を許可するため、制約は設定しません

-- 4-2. application は空文字OK（制約なし）
-- 空文字を許可するため、制約は設定しません

-- 4-3. extension と application の複合一意制約（重複不可）
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT unique_extension_application 
UNIQUE (extension, application);

-- 5. タイムスタンプ制約
-- 5-1. created_at 未来日付禁止制約
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT check_created_at_not_future 
CHECK (created_at <= NOW());

-- 5-2. updated_at 未来日付禁止制約
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT check_updated_at_not_future 
CHECK (updated_at <= NOW());

-- 5-3. touched_at 未来日付禁止制約
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT check_touched_at_not_future 
CHECK (touched_at <= NOW());

-- 5-4. updated_at が created_at より前でない制約
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT check_updated_at_after_created_at 
CHECK (updated_at >= created_at);

-- 6. パフォーマンス向上用インデックス
-- 6-1. extension インデックス
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_extension 
ON public.relation_extension_app(extension);

-- 6-2. application インデックス
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_application 
ON public.relation_extension_app(application);

-- 6-3. extension と application の複合インデックス（一意制約で自動作成されるが明示的に作成）
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_extension_application 
ON public.relation_extension_app(extension, application);

-- 6-4. created_at インデックス
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_created_at 
ON public.relation_extension_app(created_at);

-- 6-5. updated_at インデックス
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_updated_at 
ON public.relation_extension_app(updated_at);

-- 6-6. archived, disabled 複合インデックス
CREATE INDEX IF NOT EXISTS idx_relation_extension_app_archived_disabled 
ON public.relation_extension_app(archived, disabled) 
WHERE archived = false AND disabled = false;