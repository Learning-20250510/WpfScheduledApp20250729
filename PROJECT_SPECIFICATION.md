# WpfScheduledApp20250729 - プロジェクト仕様書

## 📋 プロジェクト概要

**WpfScheduledApp20250729** は、ゲーミング風UIを持つタスク管理・学習支援アプリケーションです。3層階層構造のタスク管理システム、複数の学習方法論、データ整合性監視機能を備えた包括的な生産性向上ツールです。

### 基本情報
- **プラットフォーム**: .NET 8.0 Windows (WPF)
- **データベース**: PostgreSQL
- **アーキテクチャ**: MVVM + DDD + Repository Pattern
- **UI テーマ**: Gaming/Library/Terminator/Japanese/Monochrome

---

## 🏗️ システムアーキテクチャ

### アプリケーション構造
```
WpfScheduledApp20250729/
├── Models/
│   ├── Context/           # データベースコンテキスト
│   ├── Entities/          # エンティティモデル
│   ├── FilesOperation/    # ファイル操作モデル
│   └── HowToLearn/        # 学習方法論モデル
├── Services/              # ビジネスロジック
├── ViewModels/            # MVVM ViewModel
├── Views/                 # UI Windows
├── Controls/              # カスタムコントロール
├── Auditing/              # データ整合性監査システム
├── Validation/            # バリデーション機能
├── Helpers/               # ヘルパークラス
└── Utils/                 # ユーティリティ
```

### データベース構造
- **Production**: `ScheduleApp20250729_Production`
- **Development**: `ScheduleApp20250729_Development`
- **WebPage**: `ScheduleApp20250729_WebPage`

---

## 📊 データモデル

### 3層タスク階層構造

#### 1. HighTask（高レベルタスク）
```csharp
public class HighTask : BaseEntity
{
    public int Id { get; set; }
    public int ArchitectureId { get; set; }  // 学習アーキテクチャ
    public string TaskName { get; set; }     // タスク名
    public string? Description { get; set; }
    public int ProjectId { get; set; }
    public int ClearTimesInTime { get; set; }     // 時間内クリア回数
    public int ClearTimesOutofTime { get; set; }  // 時間外クリア回数
}
```

#### 2. MiddleTask（中レベルタスク）
```csharp
public class MiddleTask : BaseEntity
{
    public int Id { get; set; }
    public int HighTaskId { get; set; }
    public string Description { get; set; }
    public string? MMFileName { get; set; }      // MindMapファイル名
    // ... その他のプロパティ
}
```

#### 3. LowTask（低レベルタスク）
```csharp
public class LowTask : BaseEntity
{
    public int Id { get; set; }
    public int MiddleTaskId { get; set; }
    public string Description { get; set; }
    public int EstimatedTime { get; set; }       // 推定時間（分）
    public DateOnly ExecutionDate { get; set; }  // 実行日
    public TimeOnly ExecutionTime { get; set; }  // 実行時刻
    public DateTime LastClearedAt { get; set; }  // 最終クリア時刻
    public string HowToLearnName { get; set; }   // 学習方法
    // ... その他のプロパティ
}
```

### マスターデータ

#### Architecture（学習アーキテクチャ）
- **Action**: 一般的なアクションタスク
- **Freeplane**: MindMap学習方式
- **Webpage**: Web学習方式  
- **PDF**: PDF学習方式

#### HowToLearn（学習方法論）
- **FreePlane系**: MindMap作成・分析
- **Movie系**: 動画学習・分析
- **PDF系**: 文書読解・OCR処理
- **WebPage系**: Web情報収集・スクレイピング
- **TheWorld系**: 実世界学習・観察

---

## 🎮 ユーザーインターフェース

### メイン画面（ReadTasksWindow）
**5つのデザインテーマ:**
1. **Gaming**: ネオンカラー、ゲーミングエフェクト
2. **Library**: 落ち着いた図書館風デザイン
3. **Terminator**: サイバー・SF風デザイン
4. **Japanese**: 和風・伝統色デザイン
5. **Monochrome**: シンプル・モノクロデザイン

### 主要機能画面
- **ReadTasksWindow**: タスク一覧・実行画面
- **AddTaskWindow**: タスク追加・MM同期画面
- **ActionTaskWindow**: タスク実行画面
- **ResultWindow**: 結果・統計表示画面
- **GamingResultWindow**: ゲーミング結果画面

### カスタムコントロール
- **ReadTaskControl**: 標準タスクリスト表示
- **LibraryTaskControl**: 図書館風タスク表示
- **HighTaskControl**: 高レベルタスク表示
- **LowTaskControl**: 低レベルタスク表示

---

## ⌨️ グローバルホットキー

| キー組み合わせ | 機能 |
|---------------|------|
| **Ctrl+R** | ウィンドウアクティベート |
| **Ctrl+Shift+T** | AddTaskWindow表示 |
| **Ctrl+Shift+1~0** | モチベーション追加 |

---

## 📁 MMファイル自動同期システム

### 概要
プロジェクト直下の`MMF`フォルダ内の`.mm`（MindMapファイル）を自動的にHighTaskテーブルと同期する機能。

### 主要機能
1. **自動INSERT**: MMファイルが存在するがDBにない → HighTaskに自動追加
2. **自動DELETE**: DBに存在するがMMファイルがない → HighTaskから自動削除
3. **定期同期**: 監査システムで30分間隔の自動実行
4. **手動同期**: AddTaskWindow の同期ボタンで即座に実行

### 設定
- **フォルダパス**: `{プロジェクトルート}/MMF/`
- **対象ファイル**: `*.mm` （サブディレクトリも再帰的に検索）
- **Architecture**: 自動的に"Freeplane"に設定
- **ProjectId**: デフォルト値1

---

## 🔍 データ整合性監査システム

### Auditingフレームワーク
自動的なデータ品質監視とメンテナンス機能。

#### 組み込み監査ルール
1. **TaskDateConsistency**: タスクの日付整合性チェック
2. **ArchivedButNotDisabled**: アーカイブ済み未無効化エンティティ検出
3. **EmptyTaskName**: 空のタスク名検出
4. **FutureCreatedAt**: 未来の作成日時検出
5. **UpdateBeforeCreate**: 更新日時が作成日時より前の検出
6. **MMFileSynchronization**: MMファイル同期（30分間隔）

#### 重要度レベル
```csharp
public enum AuditSeverity
{
    Info = 0,       // 情報（同期完了など）
    Warning = 1,    // 警告（データ不整合の可能性）
    Error = 2,      // エラー（修正が必要）
    Critical = 3    // 致命的（即座に対応必要）
}
```

#### データ不整合レコード管理
- **自動記録**: 不整合検出時に`data_inconsistency_record`テーブルに保存
- **解決追跡**: 修正状況の管理
- **自動クリーンアップ**: 30日経過した解決済みレコードの自動削除

---

## 🎯 学習方法論システム

### IHowToLearnAction インターフェース
各学習方法に共通の実行インターフェース。

#### FreePlane系学習
- **FindTasksNTimesFromTheMMFile**: MMファイルからタスク抽出
- **FocusContextOfTheMMFileWithHavingAnFreedomIdea**: 自由発想でのMM分析
- **FocusTheMMFileWithHavingAnFreedomIdeaSpeedily**: 高速MM分析

#### Movie系学習
- **FocusContextBetWeen10SecondsOfMovie**: 10秒間隔動画分析
- **FocusContextInStillImageOfMovie**: 静止画フレーム分析

#### PDF系学習
- **CreateMMFileFromContextWithOCRByAutoMMFileGeneration**: OCR + MM自動生成
- **FocusContextInSpecificPage**: 特定ページ集中学習
- **FocusDesignInSpecificPage**: ページデザイン分析

#### WebPage系学習
- **CreateMMFileAutomaticallyScrapingByAutoMMFileGeneration**: スクレイピング + MM生成
- **CreateMMFileDirectlyFromBrowserByAutoMMFileGeneration**: ブラウザ直接 + MM生成
- **FocusContextInScrollValue**: スクロール位置分析
- **FocusDesignInScrollValue**: デザイン要素分析

#### TheWorld系学習
- **AnywayOfTheWorld**: 実世界観察学習
- **ReadyForVariablesListLearningOfTheWorld**: 変数リスト学習準備

---

## 🎮 ゲーミング機能

### Result画面の統計表示
- **総タスク数**: 今日予定されたタスクの総数
- **完了タスク数**: 今日完了したタスクの数
- **残りタスク数**: 未完了タスクの数
- **推定時間**: 総推定作業時間と残り時間
- **時間内クリア回数**: 予定時間内に完了したタスク数
- **時間外クリア回数**: 予定時間を超過して完了したタスク数

### EXPシステム
- **EXP獲得**: 1タスク完了 = 100 EXP
- **プログレスバー**: 視覚的な進捗表示
- **アニメーション**: 獲得EXPの動的表示
- **完了率円グラフ**: パーセンテージ表示

### モチベーション機能
Ctrl+Shift+1~0でモチベーションメッセージを表示
- **レベル1~10**: 各種ブーストメッセージ
- **特殊メッセージ**: レベル1では特別な励ましメッセージ

---

## 🔧 技術仕様

### フレームワーク・ライブラリ
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.7" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.7" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.4" />
```

### デザインパターン
- **MVVM**: Model-View-ViewModel
- **Repository**: データアクセス抽象化
- **Service Layer**: ビジネスロジック分離
- **Command Pattern**: DelegateCommand実装
- **Observer Pattern**: NotificationObject実装

### 設定管理
- **appsettings.json**: 接続文字列・設定値
- **BaseDbContext**: マルチ環境データベース接続
- **EnvironmentContext**: Development/Production/WebPage切り替え

---

## 📱 ファイル操作システム

### サポートファイル形式
- **FreePlane**: .mm (MindMapファイル)
- **Movie**: 動画ファイル（各種形式）
- **PDF**: .pdf（OCR処理対応）
- **WebPage**: HTML/Web形式

### 自動処理機能
- **MMファイル同期**: 自動INSERT/DELETE
- **OCR処理**: PDF→テキスト変換
- **スクレイピング**: Web情報自動収集
- **MM自動生成**: コンテンツからMindMap作成

---

## 🛠️ 開発・運用

### データベースマイグレーション
```
doc/
├── AlterTable_HighTask.sql
├── AlterTable_architecture.sql
├── AlterTable_low_task.sql
├── AlterTable_middle_task.sql
├── AlterTable_periodically_cycle.sql
├── AlterTable_project.sql
├── AlterTable_relation_extension_app.sql
├── AlterTable_webpage.sql
├── insert_sample_data.sql
├── execute_all.bat
└── execute_all.ps1
```

### ログ機能
- **Utils/Logger.cs**: 統一ログ管理
- **レベル別ログ**: Info/Warning/Error/Critical
- **メソッド追跡**: 自動的なコール元記録

### バリデーション
- **ValidationManager**: 統合バリデーション管理
- **ValidationRule**: カスタムルール定義
- **ValidationRules**: 組み込みルール集

---

## 🚀 未実装機能・拡張予定

### ReadTasksWindow
- **⚡ POWER MODE**: LetsStartCheckBox機能（未実装）
  - 想定機能: 集中モード、自動タスク開始、タイマー機能

### 追加予定機能
- **タスクスケジューラー**: より詳細な時間管理
- **レポート機能**: 詳細な学習分析
- **エクスポート機能**: データ出力・バックアップ
- **プラグインシステム**: カスタム学習方法追加

### UI/UX改善
- **アクセシビリティ**: キーボード操作、スクリーンリーダー対応
- **モバイル対応**: タッチ操作サポート
- **カスタマイゼーション**: ユーザー設定UI

---

## 📞 サポート・コントリビューション

### システム要件
- **OS**: Windows 10/11
- **Framework**: .NET 8.0 Runtime
- **Database**: PostgreSQL 12+
- **メモリ**: 2GB RAM以上推奨

### 開発環境
- **IDE**: Visual Studio 2022 / VS Code
- **Database Tool**: pgAdmin, DBeaver等
- **Version Control**: Git

---

## 📝 ライセンス・著作権

このプロジェクトは学習・開発目的で作成されています。
商用利用する場合は、適切なライセンス確認を行ってください。

---

*最終更新: 2025年1月*
*バージョン: 1.0.0*
*作成者: [プロジェクトオーナー名]*