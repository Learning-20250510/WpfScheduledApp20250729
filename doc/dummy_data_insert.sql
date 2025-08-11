-- ダミーデータ投入用SQLファイル
-- 全テーブルにダミーレコードを投入

-- 1. Architecture テーブル
INSERT INTO architecture (architecture_name, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    ('Web開発アーキテクチャ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('モバイルアプリアーキテクチャ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('AIシステムアーキテクチャ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('データベースアーキテクチャ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('マイクロサービスアーキテクチャ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 2. Project テーブル
INSERT INTO project (project_name, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    ('図書館管理システム', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('ECサイト構築', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('スマートホームアプリ', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('学習管理システム', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('在庫管理システム', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 3. HowToLearn テーブル
INSERT INTO how_to_learn (htl, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    ('動画学習', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('書籍学習', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('実践プログラミング', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('オンライン講座', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('メンタリング', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 4. PeriodicallyCycle テーブル
INSERT INTO periodically_cycle (cycle, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    ('毎日', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('週3回', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('週1回', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('月2回', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('月1回', NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 5. Motivation テーブル
INSERT INTO motivation (motivation_name, description, message, icon, color, display_order, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    ('🔥燃える情熱', 'プログラマーの熱い心', 'コードで世界を変えよう！', '🔥', '#FF4444', 1, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('🎯目標達成', '明確な目標設定', '一歩一歩確実に進もう', '🎯', '#00AA00', 2, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('💡創造力', 'アイデアを形に', '新しいものを創造する喜び', '💡', '#FFD700', 3, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('🚀スピード', '素早い実行力', '迅速な行動で結果を出す', '🚀', '#0088FF', 4, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    ('🌟品質重視', '高品質なコード', '美しいコードを書こう', '⭐', '#AA00AA', 5, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 6. HighTask テーブル
INSERT INTO high_task (architecture_id, taskname, description, project_id, clear_times_intime, clear_times_outoftime, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    (1, 'データベース設計', 'ERダイアグラムの作成とテーブル設計', 1, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (2, 'UI/UX設計', 'ユーザーインターフェースの設計と検証', 2, 1, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (3, 'API設計', 'RESTful APIの設計と仕様書作成', 3, 0, 1, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (4, 'セキュリティ検証', 'アプリケーションのセキュリティ診断', 4, 2, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (5, 'パフォーマンス最適化', 'システムの性能向上と最適化', 5, 1, 1, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 7. MiddleTask テーブル
INSERT INTO middle_task (high_task_id, htl_id, project_id, description, can_auto_reschedule, estimated_time, lastcleared_at, clear_times_intime, clear_times_outoftime, periodically_cycles_id, file_name, url, specified_page_as_pdf, specified_scroll_amount_as_url, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    (1, 1, 1, 'データベース正規化の動画学習', true, 60, NOW() - INTERVAL '1 day', 1, 0, 1, 'db_normalization.mp4', 'https://example.com/db-tutorial', 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (2, 2, 2, 'デザインパターンの書籍学習', true, 90, NOW() - INTERVAL '2 days', 0, 1, 2, 'design_patterns.pdf', '', 45, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (3, 3, 3, 'Node.js実践プログラミング', false, 120, NULL, 0, 0, 3, 'nodejs_project', '', 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (4, 4, 4, 'セキュリティオンライン講座', true, 75, NOW() - INTERVAL '3 days', 2, 0, 2, '', 'https://security-course.com', 0, 500, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (5, 5, 5, 'パフォーマンステストメンタリング', false, 45, NOW() - INTERVAL '1 hour', 1, 1, 4, '', '', 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- 8. LowTask テーブル
INSERT INTO low_task (middle_task_id, project_id, estimated_time, description, execution_date, execution_time, can_auto_reschedule, lastcleared_at, clear_times_intime, clear_times_outoftime, created_at, updated_at, touched_at, last_touched_method_name, archived, disabled) 
VALUES 
    (1, 1, 30, 'データベース正規化：第1正規形の理解', CURRENT_DATE + INTERVAL '1 day', '09:00:00', true, CURRENT_TIMESTAMP - INTERVAL '2 hours', 1, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (2, 2, 45, 'Singletonパターンの実装練習', CURRENT_DATE, '14:00:00', true, CURRENT_TIMESTAMP - INTERVAL '1 day', 0, 1, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (3, 3, 60, 'Express.jsでHello Worldサーバー構築', CURRENT_DATE + INTERVAL '2 days', '10:30:00', false, CURRENT_TIMESTAMP - INTERVAL '3 days', 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (4, 4, 25, 'SQLインジェクション対策の学習', CURRENT_DATE, '16:00:00', true, CURRENT_TIMESTAMP - INTERVAL '4 hours', 2, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (5, 5, 40, 'CPU使用率監視ツールの設定', CURRENT_DATE + INTERVAL '3 days', '11:00:00', false, CURRENT_TIMESTAMP - INTERVAL '30 minutes', 1, 1, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (1, 1, 35, 'データベース正規化：第2正規形の理解', CURRENT_DATE + INTERVAL '4 days', '09:30:00', true, NULL, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (2, 2, 50, 'Observerパターンの実装練習', CURRENT_DATE + INTERVAL '1 day', '15:00:00', true, NULL, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (3, 3, 90, 'MongoDB連携APIの開発', CURRENT_DATE + INTERVAL '5 days', '13:00:00', false, NULL, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (4, 4, 30, 'CSRF攻撃対策の実装', CURRENT_DATE + INTERVAL '2 days', '17:30:00', true, NULL, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false),
    (5, 5, 55, 'メモリリーク検出と修正', CURRENT_DATE + INTERVAL '6 days', '10:00:00', false, NULL, 0, 0, NOW(), NOW(), NOW(), 'DummyDataInsert', false, false);

-- データ確認用クエリ（コメント化）
/*
SELECT COUNT(*) as architecture_count FROM architecture;
SELECT COUNT(*) as project_count FROM project;
SELECT COUNT(*) as how_to_learn_count FROM how_to_learn;
SELECT COUNT(*) as periodically_cycle_count FROM periodically_cycle;
SELECT COUNT(*) as motivation_count FROM motivation;
SELECT COUNT(*) as high_task_count FROM high_task;
SELECT COUNT(*) as middle_task_count FROM middle_task;
SELECT COUNT(*) as low_task_count FROM low_task;

-- 関連を含む詳細データ確認
SELECT h.taskname, p.project_name, a.architecture_name 
FROM high_task h 
JOIN project p ON h.project_id = p.id 
JOIN architecture a ON h.architecture_id = a.id;

SELECT m.description, h.taskname, htl.htl, pc.cycle 
FROM middle_task m 
JOIN high_task h ON m.high_task_id = h.id 
JOIN how_to_learn htl ON m.htl_id = htl.id 
JOIN periodically_cycle pc ON m.periodically_cycles_id = pc.id;

SELECT l.description, m.description as middle_task_desc, l.execution_date, l.execution_time 
FROM low_task l 
JOIN middle_task m ON l.middle_task_id = m.id 
ORDER BY l.execution_date, l.execution_time;
*/