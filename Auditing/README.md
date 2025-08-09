# データ不整合監査システム

このシステムは、バックグラウンドでデータベースの不整合をチェックし、発見した問題をログ出力およびデータベースに記録するシステムです。

## 主な機能

### 1. バックグラウンド監査
- **UIスレッドをブロックしない**: `Timer`とバックグラウンドタスクを使用してUIに影響なし
- **定期実行**: デフォルト30分間隔で自動実行（変更可能）
- **自動開始**: アプリケーション起動時に自動的に開始

### 2. 組み込み監査ルール
- **日付整合性チェック**: CreatedAt ≤ UpdatedAt ≤ TouchedAt の順序
- **アーカイブ状態チェック**: アーカイブ済みで無効化されていないエンティティ
- **空の値チェック**: 空のタスク名や説明
- **未来日付チェック**: 未来の作成日時
- **日付順序チェック**: 更新日時が作成日時より前

### 3. 複数テーブル間監査
- **参照整合性チェック**: 外部キー参照の整合性
- **階層整合性チェック**: HighTask → MiddleTask → LowTask の階層
- **プロジェクト整合性**: プロジェクトとタスクの整合性
- **数値フィールド整合性**: 負の値チェック

### 4. 不整合レコード管理
- **自動保存**: 発見した不整合を`DataInconsistencyRecord`テーブルに保存
- **重複防止**: 同じ不整合の重複記録を防止
- **解決管理**: 不整合の解決状態を追跡
- **統計情報**: 重要度別の統計をログ出力

### 5. ログ出力
- **詳細なログ**: 既存のLoggerシステムを使用
- **重要度分類**: Info/Warning/Error/Critical で分類
- **統計情報**: 監査結果の統計をログ出力

## システム構造

```
Auditing/
├── Interfaces/          # インターフェース定義
│   ├── IAuditRule.cs
│   ├── IAuditService.cs
│   └── IAuditRuleFactory.cs
├── Models/              # データモデル
│   └── AuditResult.cs
├── Rules/               # 監査ルール基底クラス
│   ├── BaseAuditRule.cs
│   └── DynamicAuditRule.cs
├── Services/            # サービス実装
│   ├── AuditService.cs
│   ├── AuditManager.cs
│   └── AuditRuleFactory.cs
└── Examples/            # カスタムルール例
    └── CustomRuleExamples.cs
```

## 使用方法

### 1. 基本的な使用（自動）
システムは`App.xaml.cs`で自動的に初期化・開始されます：

```csharp
// App.xaml.cs で自動実行
private AuditManager? _auditManager;

protected override async void OnStartup(StartupEventArgs e)
{
    // ... 既存の初期化コード ...
    
    // 監査システムを初期化して開始
    await InitializeAuditSystemAsync(dbContext);
}
```

### 2. カスタムルールの追加

```csharp
// AuditManagerを取得してカスタムルールを追加
auditManager.AddCustomRule(
    "MyCustomRule",
    "独自のビジネスルール説明",
    async (context) =>
    {
        var results = new List<AuditResult>();
        
        // 独自の検証ロジック
        if (context is BaseDbContext dbContext)
        {
            var problemRecords = await dbContext.SomeTable
                .Where(/* 条件 */)
                .ToListAsync();
                
            foreach (var record in problemRecords)
            {
                results.Add(new AuditResult
                {
                    RuleName = "MyCustomRule",
                    EntityType = "SomeTable",
                    EntityId = record.Id.ToString(),
                    Description = "問題の説明",
                    Severity = AuditSeverity.Warning,
                    DetectedAt = DateTime.Now,
                    Details = "詳細情報"
                });
            }
        }
        
        return results;
    }
);
```

### 3. 監査間隔の変更

```csharp
// 10分間隔に変更
auditManager.SetInterval(TimeSpan.FromMinutes(10));
```

### 4. ルールの削除

```csharp
// 不要なルールを削除
auditManager.RemoveRule("RuleName");
```

## 複数テーブル間監査の例

### 参照整合性チェック
```csharp
// MiddleTaskが存在しないHighTaskIdを参照している場合を検出
var invalidReferences = await dbContext.MiddleTasks
    .Where(mt => !dbContext.HighTasks.Any(ht => ht.Id == mt.HighTaskId))
    .ToListAsync();
```

### 階層整合性チェック
```csharp
// プロジェクトIDの一貫性をチェック
var inconsistentTasks = await (from mt in dbContext.MiddleTasks
                              join ht in dbContext.HighTasks on mt.HighTaskId equals ht.Id
                              where mt.ProjectId != ht.ProjectId
                              select new { MiddleTask = mt, HighTask = ht })
                              .ToListAsync();
```

## データ不整合レコード管理

### 未解決レコードの取得
```csharp
var service = new DataInconsistencyRecordService(dbContext);
var unresolvedRecords = await service.GetUnresolvedRecordsAsync();
```

### レコードを解決済みにマーク
```csharp
await service.MarkAsResolvedAsync(recordId, "管理者名", "解決方法の説明");
```

### 重要度別の取得
```csharp
var criticalRecords = await service.GetRecordsBySeverityAsync(AuditSeverity.Critical);
```

## ログ出力例

```
2025-08-09 10:00:00.123 [INFO] [AuditService] Starting audit execution with 8 rules
2025-08-09 10:00:00.234 [INFO] [AuditService] Rule 'TaskDateConsistency' found 2 issues
2025-08-09 10:00:00.345 [INFO] [AuditService] Rule 'EmptyTaskName' found no issues
2025-08-09 10:00:00.456 [INFO] [AuditService] === AUDIT RESULTS ===
2025-08-09 10:00:00.567 [ERROR] [AuditService] [Critical] ReferentialIntegrityCheck: 存在しないHighTaskId '999' を参照しています (Entity: MiddleTask#123)
2025-08-09 10:00:00.678 [INFO] [AuditService] === END AUDIT RESULTS ===
2025-08-09 10:00:00.789 [INFO] [AuditService] 不整合レコードをデータベースに保存中: 2件
2025-08-09 10:00:00.890 [INFO] [AuditService] 不整合レコード保存完了: 2件
2025-08-09 10:00:00.901 [INFO] [AuditService]   - Critical: 1件
2025-08-09 10:00:00.912 [INFO] [AuditService]   - Warning: 1件
2025-08-09 10:00:00.923 [INFO] [AuditService] 現在の未解決不整合レコード総数: 15件
```

## パフォーマンス考慮事項

- **非ブロッキング**: UIスレッドに影響なし
- **バッチ処理**: 不整合レコードはバッチで効率的に保存
- **重複防止**: 既存レコードの更新で無駄な保存を回避
- **エラー処理**: データベース保存失敗時もログ出力は継続

## 拡張性

システムは高い拡張性を持ちます：
- **新しいルールの追加**: `IAuditRule`を実装して動的追加
- **カスタムチェック**: ビジネス固有の検証ルールを簡単に追加
- **通知機能**: `AuditCompleted`イベントで外部システム連携
- **レポート機能**: 不整合レコードからレポート生成可能