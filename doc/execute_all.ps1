$files = @(
    "AlterTable_architecture.sql",
    "AlterTable_HighTask.sql",
    "AlterTable_low_task.sql",
    "AlterTable_middle_task.sql",
    "AlterTable_periodically_cycle.sql",
    "AlterTable_project.sql",
    "AlterTable_relation_extension_app.sql",
    "AlterTable_webpage.sql",
    "CreateTable_motivation.sql",
    "Insert_motivation_data.sql"
)

Write-Host "PostgreSQL一括実行開始..." -ForegroundColor Green
Set-Location $PSScriptRoot

foreach ($file in $files) {
    Write-Host "実行中: $file" -ForegroundColor Yellow
    & psql -h localhost -U postgres -d ScheduleApp20250729_Development -f $file
    if ($LASTEXITCODE -eq 0) {
        Write-Host "成功: $file" -ForegroundColor Green
    } else {
        Write-Host "エラー: $file" -ForegroundColor Red
    }
}

Write-Host "実行完了" -ForegroundColor Green
Read-Host "Press any key to continue"