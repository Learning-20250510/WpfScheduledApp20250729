-- ========================================
-- low_task テーブル制約・初期値設定
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1-1. estimated_time のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN estimated_time SET DEFAULT 1;

-- 1-2. description のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN description SET DEFAULT null;

-- 1-3. can_auto_reschedule のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN can_auto_reschedule SET DEFAULT false;

-- 1-4. lastcleared_at のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN lastcleared_at SET DEFAULT null;

-- 1-5. clear_times_intime のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN clear_times_intime SET DEFAULT 0;

-- 1-6. clear_times_outoftime のデフォルト値設定
ALTER TABLE public.low_task 
ALTER COLUMN clear_times_outoftime SET DEFAULT 0;

-- 1-7. execution_date のデフォルト値設定（NULL許可）
ALTER TABLE public.low_task 
ALTER COLUMN execution_date SET DEFAULT null;

-- 1-8. execution_time のデフォルト値設定（NULL許可）
ALTER TABLE public.low_task 
ALTER COLUMN execution_time SET DEFAULT null;

-- 2-1. middle_task_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.low_task 
ADD CONSTRAINT fk_low_task_middle_task_id 
FOREIGN KEY (middle_task_id) REFERENCES public.middle_task(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 2-2. project_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.low_task 
ADD CONSTRAINT fk_low_task_project_id 
FOREIGN KEY (project_id) REFERENCES public.project(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 3-1. estimated_time 1以上制約
ALTER TABLE public.low_task 
ADD CONSTRAINT check_estimated_time_positive 
CHECK (estimated_time >= 1);

-- 3-2. clear_times_intime 0以上制約
ALTER TABLE public.low_task 
ADD CONSTRAINT check_clear_times_intime_non_negative 
CHECK (clear_times_intime >= 0);

-- 3-3. clear_times_outoftime 0以上制約
ALTER TABLE public.low_task 
ADD CONSTRAINT check_clear_times_outoftime_non_negative 
CHECK (clear_times_outoftime >= 0);

-- 3-4. description 空文字禁止制約（NULL許可、空文字は禁止）
ALTER TABLE public.low_task 
ADD CONSTRAINT check_description_not_empty 
CHECK (description IS NULL OR char_length(trim(description)) > 0);

-- 4-1. execution_date 有効日付制約（必要に応じて）
-- PostgreSQLのDATE型は自動的にYYYY-MM-DD形式を保証するため、フォーマット制約は不要
-- ALTER TABLE public.low_task 
-- ADD CONSTRAINT check_execution_date_valid 
-- CHECK (execution_date IS NULL OR execution_date >= '1900-01-01');

-- 4-2. execution_time 有効時刻制約（必要に応じて）
-- PostgreSQLのTIME型は自動的にHH:MM:SS形式を保証するため、フォーマット制約は不要
-- ALTER TABLE public.low_task 
-- ADD CONSTRAINT check_execution_time_valid 
-- CHECK (execution_time IS NULL OR (
--     EXTRACT(HOUR FROM execution_time) BETWEEN 0 AND 23 AND
--     EXTRACT(MINUTE FROM execution_time) BETWEEN 0 AND 59
-- ));

-- 5-1. パフォーマンス向上用インデックス - middle_task_id
CREATE INDEX IF NOT EXISTS idx_low_task_middle_task_id 
ON public.low_task(middle_task_id);

-- 5-2. パフォーマンス向上用インデックス - project_id
CREATE INDEX IF NOT EXISTS idx_low_task_project_id 
ON public.low_task(project_id);

-- 5-3. execution_date インデックス
CREATE INDEX IF NOT EXISTS idx_low_task_execution_date 
ON public.low_task(execution_date);

-- 5-4. execution_time インデックス
CREATE INDEX IF NOT EXISTS idx_low_task_execution_time 
ON public.low_task(execution_time);

-- 5-5. lastcleared_at インデックス
CREATE INDEX IF NOT EXISTS idx_low_task_lastcleared_at 
ON public.low_task(lastcleared_at);

-- 5-6. middle_task_id と project_id の複合インデックス
CREATE INDEX IF NOT EXISTS idx_low_task_middle_project 
ON public.low_task(middle_task_id, project_id);

-- 5-7. 実行予定日時の複合インデックス
CREATE INDEX IF NOT EXISTS idx_low_task_execution_datetime 
ON public.low_task(execution_date, execution_time) 
WHERE execution_date IS NOT NULL AND execution_time IS NOT NULL;