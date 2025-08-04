-- ========================================
-- middle_task テーブル制約・初期値設定
-- 以下のSQLを1つずつ実行してください（A5M2対応）
-- ========================================

-- 1. デフォルト値設定
-- 1-1. description のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN description SET DEFAULT null;

-- 1-2. can_auto_reschedule のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN can_auto_reschedule SET DEFAULT false;

-- 1-3. estimated_time のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN estimated_time SET DEFAULT 1;

-- 1-4. lastcleared_at のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN lastcleared_at SET DEFAULT null;

-- 1-5. clear_times_intime のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN clear_times_intime SET DEFAULT 0;

-- 1-6. clear_times_outoftime のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN clear_times_outoftime SET DEFAULT 0;

-- 1-7. file_name のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN file_name SET DEFAULT null;

-- 1-8. url のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN url SET DEFAULT null;

-- 1-9. specified_page_as_pdf のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN specified_page_as_pdf SET DEFAULT 0;

-- 1-10. specified_scroll_amount_as_url のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN specified_scroll_amount_as_url SET DEFAULT 0;

-- 1-11. created_at のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN created_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-12. updated_at のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN updated_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-13. touched_at のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN touched_at SET DEFAULT CURRENT_TIMESTAMP;

-- 1-14. last_touched_method_name のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN last_touched_method_name SET DEFAULT null;

-- 1-15. error_message のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN error_message SET DEFAULT null;

-- 1-16. archived のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN archived SET DEFAULT false;

-- 1-17. disabled のデフォルト値設定
ALTER TABLE public.middle_task 
ALTER COLUMN disabled SET DEFAULT false;

-- 2. 更新トリガー関数の作成（middle_task用）
CREATE OR REPLACE FUNCTION update_middle_task_timestamps()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    NEW.touched_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- 3-1. 既存のトリガーを削除（存在する場合）
DROP TRIGGER IF EXISTS trigger_update_middle_task_timestamps ON public.middle_task;

-- 3-2. 更新トリガーの作成
CREATE TRIGGER trigger_update_middle_task_timestamps
    BEFORE UPDATE ON public.middle_task
    FOR EACH ROW
    EXECUTE FUNCTION update_middle_task_timestamps();

-- 4. 外部キー制約
-- 4-1. high_task_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.middle_task 
ADD CONSTRAINT fk_middle_task_high_task_id 
FOREIGN KEY (high_task_id) REFERENCES public.high_task(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 4-2. htl_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.middle_task 
ADD CONSTRAINT fk_middle_task_htl_id 
FOREIGN KEY (htl_id) REFERENCES public.how_to_learn(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 4-3. project_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.middle_task 
ADD CONSTRAINT fk_middle_task_project_id 
FOREIGN KEY (project_id) REFERENCES public.project(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 4-4. periodically_cycles_id 外部キー制約（削除禁止、更新時カスケード）
ALTER TABLE public.middle_task 
ADD CONSTRAINT fk_middle_task_periodically_cycles_id 
FOREIGN KEY (periodically_cycles_id) REFERENCES public.periodically_cycle(id)
ON DELETE RESTRICT 
ON UPDATE CASCADE;

-- 5. チェック制約
-- 5-1. estimated_time 1以上制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_estimated_time_positive 
CHECK (estimated_time >= 1);

-- 5-2. clear_times_intime 0以上制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_clear_times_intime_non_negative 
CHECK (clear_times_intime >= 0);

-- 5-3. clear_times_outoftime 0以上制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_clear_times_outoftime_non_negative 
CHECK (clear_times_outoftime >= 0);

-- 5-4. specified_page_as_pdf 0以上制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_specified_page_as_pdf_non_negative 
CHECK (specified_page_as_pdf >= 0);

-- 5-5. specified_scroll_amount_as_url 0以上制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_specified_scroll_amount_as_url_non_negative 
CHECK (specified_scroll_amount_as_url >= 0);

-- 5-6. description 空文字禁止制約（NULL許可、空文字は禁止）
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_description_not_empty 
CHECK (description IS NULL OR char_length(trim(description)) > 0);

-- 5-7. file_name 空文字禁止制約（NULL許可、空文字は禁止）
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_file_name_not_empty 
CHECK (file_name IS NULL OR char_length(trim(file_name)) > 0);

-- 5-8. url 空文字禁止制約（NULL許可、空文字は禁止）
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_url_not_empty 
CHECK (url IS NULL OR char_length(trim(url)) > 0);

-- 6. タイムスタンプ制約
-- 6-1. created_at 未来日付禁止制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_created_at_not_future 
CHECK (created_at <= CURRENT_TIMESTAMP);

-- 6-2. updated_at 未来日付禁止制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_updated_at_not_future 
CHECK (updated_at <= CURRENT_TIMESTAMP);

-- 6-3. touched_at 未来日付禁止制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_touched_at_not_future 
CHECK (touched_at <= CURRENT_TIMESTAMP);

-- 6-4. updated_at が created_at より前でない制約
ALTER TABLE public.middle_task 
ADD CONSTRAINT check_updated_at_after_created_at 
CHECK (updated_at >= created_at);

-- 7. パフォーマンス向上用インデックス
-- 7-1. high_task_id インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_high_task_id 
ON public.middle_task(high_task_id);

-- 7-2. htl_id インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_htl_id 
ON public.middle_task(htl_id);

-- 7-3. project_id インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_project_id 
ON public.middle_task(project_id);

-- 7-4. periodically_cycles_id インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_periodically_cycles_id 
ON public.middle_task(periodically_cycles_id);

-- 7-5. created_at インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_created_at 
ON public.middle_task(created_at);

-- 7-6. updated_at インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_updated_at 
ON public.middle_task(updated_at);

-- 7-7. lastcleared_at インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_lastcleared_at 
ON public.middle_task(lastcleared_at);

-- 7-8. archived, disabled 複合インデックス
CREATE INDEX IF NOT EXISTS idx_middle_task_archived_disabled 
ON public.middle_task(archived, disabled) 
WHERE archived = false AND disabled = false;

-- 7-9. 複合インデックス（よく一緒に検索される組み合わせ）
CREATE INDEX IF NOT EXISTS idx_middle_task_high_task_project 
ON public.middle_task(high_task_id, project_id);

-- 7-10. 複合インデックス（学習方法とプロジェクト）
CREATE INDEX IF NOT EXISTS idx_middle_task_htl_project 
ON public.middle_task(htl_id, project_id);