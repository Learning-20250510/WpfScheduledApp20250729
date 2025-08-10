-- ========================================
-- high_task テーブル制約・初期値設定（修正版）
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1-1. created_at のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN created_at SET DEFAULT NOW();

-- 1-2. updated_at のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN updated_at SET DEFAULT NOW();

-- 1-3. touched_at のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN touched_at SET DEFAULT NOW();

-- 1-4. last_touched_method_name のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN last_touched_method_name SET DEFAULT null;

-- 1-5. error_message のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN error_message SET DEFAULT null;

-- 1-6. archived のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN archived SET DEFAULT false;

-- 1-7. disabled のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN disabled SET DEFAULT false;

-- 1-8. description のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN description SET DEFAULT null;

-- 1-9. clear_times_intime のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN clear_times_intime SET DEFAULT 0;

-- 1-10. clear_times_outoftime のデフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN clear_times_outoftime SET DEFAULT 0;

-- 2. 更新トリガー関数の作成（high_task用）
CREATE OR REPLACE FUNCTION update_high_task_timestamps()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = NOW();
    NEW.touched_at = NOW();
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- 3-1. 既存のトリガーを削除（存在する場合）
DROP TRIGGER IF EXISTS trigger_update_high_task_timestamps ON public.high_task;

-- 3-2. 更新トリガーの作成
CREATE TRIGGER trigger_update_high_task_timestamps
    BEFORE UPDATE ON public.high_task
    FOR EACH ROW
    EXECUTE FUNCTION update_high_task_timestamps();

-- 4-1. architecture_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.high_task 
ADD CONSTRAINT fk_high_task_architecture_id 
FOREIGN KEY (architecture_id) REFERENCES public.architecture(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 4-2. project_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.high_task 
ADD CONSTRAINT fk_high_task_project_id 
FOREIGN KEY (project_id) REFERENCES public.project(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 5-1. taskname 空文字禁止制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_taskname_not_empty 
CHECK (char_length(trim(taskname)) > 0);

-- 5-2. clear_times_intime 0以上制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_clear_times_intime_non_negative 
CHECK (clear_times_intime >= 0);

-- 5-3. clear_times_outoftime 0以上制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_clear_times_outoftime_non_negative 
CHECK (clear_times_outoftime >= 0);

-- 5-4. architecture_id != 1 の場合のtaskname一意制約
-- 部分一意インデックスを使用
CREATE UNIQUE INDEX unique_taskname_when_architecture_not_1 
ON public.high_task (taskname) 
WHERE architecture_id != 1;

-- 6-1. created_at 未来日付禁止制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_created_at_not_future 
CHECK (created_at <= NOW());

-- 6-2. updated_at 未来日付禁止制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_updated_at_not_future 
CHECK (updated_at <= NOW());

-- 6-3. touched_at 未来日付禁止制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_touched_at_not_future 
CHECK (touched_at <= NOW());

-- 6-4. updated_at が created_at より前でない制約
ALTER TABLE public.high_task 
ADD CONSTRAINT check_updated_at_after_created_at 
CHECK (updated_at >= created_at);

-- 7-1. パフォーマンス向上用インデックス - architecture_id
CREATE INDEX IF NOT EXISTS idx_high_task_architecture_id 
ON public.high_task(architecture_id);

-- 7-2. パフォーマンス向上用インデックス - project_id
CREATE INDEX IF NOT EXISTS idx_high_task_project_id 
ON public.high_task(project_id);

-- 7-3. created_at インデックス
CREATE INDEX IF NOT EXISTS idx_high_task_created_at 
ON public.high_task(created_at);

-- 7-4. updated_at インデックス
CREATE INDEX IF NOT EXISTS idx_high_task_updated_at 
ON public.high_task(updated_at);

-- 7-5. taskname インデックス（検索用）
CREATE INDEX IF NOT EXISTS idx_high_task_taskname 
ON public.high_task(taskname);

-- 7-6. archived, disabled 複合インデックス
CREATE INDEX IF NOT EXISTS idx_high_task_archived_disabled 
ON public.high_task(archived, disabled) 
WHERE archived = false AND disabled = false;

-- 7-7. architecture_id と project_id の複合インデックス
CREATE INDEX IF NOT EXISTS idx_high_task_architecture_project 
ON public.high_task(architecture_id, project_id);