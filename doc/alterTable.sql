-- 複合一意制約を追加
ALTER TABLE public.relation_extension_app 
ADD CONSTRAINT UK_relation_extension_app_ext_app 
UNIQUE (extension, application);

-- htlカラムに一意制約を追加
ALTER TABLE public.how_to_learn 
ADD CONSTRAINT UK_how_to_learn_htl 
UNIQUE (htl);

-- 1. 外部キー制約
ALTER TABLE public.high_task 
ADD CONSTRAINT FK_high_task_architecture_id 
FOREIGN KEY (architecture_id) 
REFERENCES public.architecture(id) 
ON DELETE CASCADE;

-- 2. 条件付き一意制約（architecture_id != 1の場合のみtasknameユニーク）
CREATE UNIQUE INDEX UK_high_task_taskname_conditional 
ON public.high_task (taskname) 
WHERE architecture_id != 1;

-- 3. 数値チェック制約
ALTER TABLE public.high_task 
ADD CONSTRAINT CK_high_task_clear_times 
CHECK (clear_times_intime >= 0 AND clear_times_outoftime >= 0);

-- 4. デフォルト値設定
ALTER TABLE public.high_task 
ALTER COLUMN clear_times_intime SET DEFAULT 0;

ALTER TABLE public.high_task 
ALTER COLUMN clear_times_outoftime SET DEFAULT 0;

-- created_at のデフォルト値を現在時刻に設定
ALTER TABLE public.high_task 
ALTER COLUMN created_at SET DEFAULT CURRENT_TIMESTAMP;

-- updated_at のデフォルト値を現在時刻に設定
ALTER TABLE public.high_task 
ALTER COLUMN updated_at SET DEFAULT CURRENT_TIMESTAMP;

-- touched_at のデフォルト値を現在時刻に設定
ALTER TABLE public.high_task 
ALTER COLUMN touched_at SET DEFAULT CURRENT_TIMESTAMP;

-- updated_at自動更新用の関数を作成
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS '
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
' LANGUAGE plpgsql;

-- high_taskテーブルにトリガーを設定
CREATE TRIGGER update_high_task_updated_at
    BEFORE UPDATE ON public.high_task
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();
    
-- archived のデフォルト値を false に設定
ALTER TABLE public.high_task 
ALTER COLUMN archived SET DEFAULT false;

-- disabled のデフォルト値を false に設定
ALTER TABLE public.high_task 
ALTER COLUMN disabled SET DEFAULT false;




