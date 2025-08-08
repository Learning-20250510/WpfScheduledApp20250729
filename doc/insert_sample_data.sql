-- サンプルデータ作成（外部キー制約対応）

-- 1. Architectureテーブルにサンプルデータ
INSERT INTO architecture (created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    (NOW(), NOW(), NOW(), 'InitialData', false, false),
    (NOW(), NOW(), NOW(), 'InitialData', false, false)
ON CONFLICT DO NOTHING;

-- 2. Projectテーブルにサンプルデータ  
INSERT INTO project (created_at, updated_at, touched_at, last_touched_method_name, archived, disabled)
VALUES 
    (NOW(), NOW(), NOW(), 'InitialData', false, false),
    (NOW(), NOW(), NOW(), 'InitialData', false, false)
ON CONFLICT DO NOTHING;

-- データ確認用クエリ
-- SELECT id FROM architecture LIMIT 5;
-- SELECT id FROM project LIMIT 5;