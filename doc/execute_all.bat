@echo off
echo PostgreSQL一括実行開始...
cd /d "%~dp0"

psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_architecture.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_HighTask.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_low_task.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_middle_task.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_periodically_cycle.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_project.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_relation_extension_app.sql"
psql -h localhost -U postgres -d ScheduleApp20250729_Development -f "AlterTable_webpage.sql"

echo 実行完了
pause