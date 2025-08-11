using WpfScheduledApp20250729.Models.Context;
using System;

namespace WpfScheduledApp20250729.Services
{
    internal class DataSeederService
    {
        private readonly BaseDbContext _context;
        private readonly ArchitectureService _architectureService;
        private readonly ProjectService _projectService;
        private readonly HowToLearnService _howToLearnService;
        private readonly PeriodicallyCycleService _periodicallyCycleService;
        private readonly RelationExtensionAppService _relationExtensionAppService;
        private readonly HighTaskService _highTaskService;
        private readonly MotivationService _motivationService;

        public DataSeederService(
            BaseDbContext context,
            ArchitectureService architectureService,
            ProjectService projectService,
            HowToLearnService howToLearnService,
            PeriodicallyCycleService periodicallyCycleService,
            RelationExtensionAppService relationExtensionAppService,
            HighTaskService highTaskService,
            MotivationService motivationService)
        {
            _context = context;
            _architectureService = architectureService;
            _projectService = projectService;
            _howToLearnService = howToLearnService;
            _periodicallyCycleService = periodicallyCycleService;
            _relationExtensionAppService = relationExtensionAppService;
            _highTaskService = highTaskService;
            _motivationService = motivationService;
        }

        /// <summary>
        /// アプリ起動時に全テーブルの初期データを作成
        /// </summary>
        public async Task SeedInitialDataAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== 初期データ作成開始 ===");

                // 1. Architecture初期データ
                try
                {
                    await SeedArchitecturesAsync();
                    System.Diagnostics.Debug.WriteLine("Architecture初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== Architecture作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    throw;
                }

                // 2. Project初期データ
                try
                {
                    await SeedProjectsAsync();
                    System.Diagnostics.Debug.WriteLine("Project初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== Project作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    throw;
                }

                // 3. HowToLearn初期データ
                try
                {
                    await SeedHowToLearnsAsync();
                    System.Diagnostics.Debug.WriteLine("HowToLearn初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== HowToLearn作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    throw;
                }

                // 4. PeriodicallyCycle初期データ
                try
                {
                    await SeedPeriodicallyCyclesAsync();
                    System.Diagnostics.Debug.WriteLine("PeriodicallyCycle初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== PeriodicallyCycle作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    throw;
                }

                // 5. RelationExtensionApp初期データ
                try
                {
                    await SeedRelationExtensionAppsAsync();
                    System.Diagnostics.Debug.WriteLine("RelationExtensionApp初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== RelationExtensionApp作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    throw;
                }

                // 6. Motivation初期データ
                try
                {
                    await SeedMotivationsAsync();
                    System.Diagnostics.Debug.WriteLine("Motivation初期データ作成完了");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"=== Motivation作成エラー ===");
                    System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                    throw;
                }

                System.Diagnostics.Debug.WriteLine("=== 初期データ作成完了 ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== 初期データ作成総合エラー ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task SeedArchitecturesAsync()
        {
            var defaultArchitectures = new[]
            {
                "Action",
                "Freeplane",
                "Webpage", 
                "PDF"
            };

            foreach (var name in defaultArchitectures)
            {
                var existing = await _architectureService.GetArchitectureByNameAsync(name);
                if (existing == null)
                {
                    var newArchitecture = new Models.Entities.Architecture
                    {
                        ArchitectureName = name,
                        LastUpdMethodName = "SeedArchitectures"
                    };
                    await _architectureService.AddArchitectureAsync(newArchitecture);
                    System.Diagnostics.Debug.WriteLine($"Architecture作成: {name}");
                }
            }
        }

        private async Task SeedProjectsAsync()
        {
            var defaultProjects = new[]
            {
                "Unclassified",
            };

            foreach (var name in defaultProjects)
            {
                // BaseServiceのメソッドを使用してチェック
                var allProjects = await _projectService.GetAllAsync();
                var existing = allProjects.FirstOrDefault(p => p.ProjectName == name);
                if (existing == null)
                {
                    var newProject = new Models.Entities.Project
                    {
                        ProjectName = name,
                        LastUpdMethodName = "SeedProjects"
                    };
                    await _projectService.AddAsync(newProject);
                    System.Diagnostics.Debug.WriteLine($"Project作成: {name}");
                }
            }
        }

        private async Task SeedHowToLearnsAsync()
        {
            var defaultHowToLearns = new[]
            {
                "LearnFromTheWorld"
            };

            foreach (var name in defaultHowToLearns)
            {
                var allHowToLearns = await _howToLearnService.GetAllAsync();
                var existing = allHowToLearns.FirstOrDefault(h => h.Htl == name);
                if (existing == null)
                {
                    var newHowToLearn = new Models.Entities.HowToLearn
                    {
                        Htl = name,
                        LastUpdMethodName = "SeedHowToLearns"
                    };
                    await _howToLearnService.AddAsync(newHowToLearn);
                    System.Diagnostics.Debug.WriteLine($"HowToLearn作成: {name}");
                }
            }
        }

        private async Task SeedPeriodicallyCyclesAsync()
        {
            var defaultCycles = new[]
            {
                "One-time",
                "Daily",
                "Weekly Monday",
                "Weekly Tuesday",
                "Weekly Wednesday",
                "Weekly Thursday",
                "Weekly Friday",
                "Weekly Saturday",
                "Weekly Sunday",
                "Monthly 1st Monday",
                "Monthly 2nd Monday",
                "Monthly 3rd Monday",
                "Monthly 4th Monday",
                "Monthly 1st Tuesday",
                "Monthly 2nd Tuesday",
                "Monthly 3rd Tuesday",
                "Monthly 4th Tuesday",
                "Monthly 1st Wednesday",
                "Monthly 2nd Wednesday",
                "Monthly 3rd Wednesday",
                "Monthly 4th Wednesday",
                "Monthly 1st Thursday",
                "Monthly 2nd Thursday",
                "Monthly 3rd Thursday",
                "Monthly 4th Thursday",
                "Monthly 1st Friday",
                "Monthly 2nd Friday",
                "Monthly 3rd Friday",
                "Monthly 4th Friday",
                "Monthly 1st Saturday",
                "Monthly 2nd Saturday",
                "Monthly 3rd Saturday",
                "Monthly 4th Saturday",
                "Monthly 1st Sunday",
                "Monthly 2nd Sunday",
                "Monthly 3rd Sunday",
                "Monthly 4th Sunday"
            };

            foreach (var name in defaultCycles)
            {
                var allCycles = await _periodicallyCycleService.GetAllAsync();
                var existing = allCycles.FirstOrDefault(c => c.Cycle == name);
                if (existing == null)
                {
                    var newCycle = new Models.Entities.PeriodicallyCycle
                    {
                        Cycle = name,
                        LastUpdMethodName = "SeedPeriodicallyCycles"
                    };
                    await _periodicallyCycleService.AddAsync(newCycle);
                    System.Diagnostics.Debug.WriteLine($"PeriodicallyCycle作成: {name}");
                }
            }
        }

        private async Task SeedRelationExtensionAppsAsync()
        {
            var defaultExtensionApps = new[]
            {
                ("", "VisualStudioCode"), // 空文字
                ("py", "VisualStudioCode"), // Python
                ("cs", "VisualStudioCode"), // C#
                ("js", "VisualStudioCode"), // JavaScript
                ("ts", "VisualStudioCode"), // TypeScript
                ("java", "VisualStudioCode"), // Java
                ("cpp", "VisualStudioCode"), // C++
                ("c", "VisualStudioCode"), // C
                ("html", "VisualStudioCode"), // HTML
                ("css", "VisualStudioCode"), // CSS
                ("scss", "VisualStudioCode"), // SCSS
                ("json", "VisualStudioCode"), // JSON
                ("xml", "VisualStudioCode"), // XML
                ("sql", "VisualStudioCode"), // SQL
                ("php", "VisualStudioCode"), // PHP
                ("rb", "VisualStudioCode"), // Ruby
                ("go", "VisualStudioCode"), // Go
                ("rs", "VisualStudioCode"), // Rust
                ("swift", "VisualStudioCode"), // Swift
                ("kt", "VisualStudioCode"), // Kotlin
                ("dart", "VisualStudioCode"), // Dart
                ("vue", "VisualStudioCode"), // Vue
                ("jsx", "VisualStudioCode"), // JSX
                ("tsx", "VisualStudioCode"), // TSX
                ("md", "VisualStudioCode"), // Markdown
                ("yml", "VisualStudioCode"), // YAML
                ("yaml", "VisualStudioCode"), // YAML
                ("toml", "VisualStudioCode"), // TOML
                ("ini", "VisualStudioCode"), // INI
                ("cfg", "VisualStudioCode"), // Config
                ("conf", "VisualStudioCode"), // Config
                ("sh", "VisualStudioCode"), // Shell
                ("bat", "VisualStudioCode"), // Batch
                ("ps1", "VisualStudioCode"), // PowerShell
                ("r", "VisualStudioCode"), // R
                ("m", "VisualStudioCode"), // Objective-C
                ("scala", "VisualStudioCode"), // Scala
                ("clj", "VisualStudioCode"), // Clojure
                ("hs", "VisualStudioCode"), // Haskell
                ("elm", "VisualStudioCode"), // Elm
                ("ex", "VisualStudioCode"), // Elixir
                ("erl", "VisualStudioCode"), // Erlang
                ("lua", "VisualStudioCode"), // Lua
                ("pl", "VisualStudioCode"), // Perl
                ("tcl", "VisualStudioCode"), // Tcl
                ("vb", "VisualStudioCode"), // Visual Basic
                ("fs", "VisualStudioCode"), // F#
                ("pas", "VisualStudioCode"), // Pascal
                ("asm", "VisualStudioCode") // Assembly
            };

            foreach (var (extension, application) in defaultExtensionApps)
            {
                var allRelations = await _relationExtensionAppService.GetAllAsync();
                var existing = allRelations.FirstOrDefault(r => r.Extension == extension);
                if (existing == null)
                {
                    var newRelation = new Models.Entities.RelationExtensionApp
                    {
                        Extension = extension,
                        Application = application,
                        LastUpdMethodName = "SeedRelationExtensionApps"
                    };
                    await _relationExtensionAppService.AddAsync(newRelation);
                    System.Diagnostics.Debug.WriteLine($"RelationExtensionApp作成: {extension} -> {application}");
                }
            }
        }

        private async Task SeedMotivationsAsync()
        {
            var defaultMotivations = new[]
            {
                new { Name = "exciting", Description = "興奮・やる気満々の状態", Message = "🎮 EXCITEMENT MODE! エネルギー全開でタスクに突撃だ！", Icon = "🚀", Color = "#FF00FF00", Order = 1 },
                new { Name = "lazy", Description = "やる気が出ない、だらけたい状態", Message = "😴 今日は少しペースダウン...でも少しずつ進もう", Icon = "😴", Color = "#FFFF8000", Order = 2 },
                new { Name = "postpone", Description = "先延ばししたい、後回しにしたい気分", Message = "⏰ 「後でやろう」って思ってる？今やっちゃおう！", Icon = "⏰", Color = "#FFFFFF00", Order = 3 },
                new { Name = "lazy-solved", Description = "だらけた状態を克服した", Message = "✨ だらけモードから復活！小さな一歩が大きな変化の始まりだ！", Icon = "✨", Color = "#FF00FFAA", Order = 4 },
                new { Name = "postpone-solved", Description = "先延ばし癖を克服した", Message = "🎯 先延ばし撃破！行動力が戻ってきた！", Icon = "🎯", Color = "#FF00AAFF", Order = 5 }
            };

            foreach (var motivation in defaultMotivations)
            {
                var existing = await _motivationService.GetMotivationByNameAsync(motivation.Name);
                if (existing == null)
                {
                    var newMotivation = new Models.Entities.Motivation
                    {
                        MotivationName = motivation.Name,
                        Description = motivation.Description,
                        Message = motivation.Message,
                        Icon = motivation.Icon,
                        Color = motivation.Color,
                        DisplayOrder = motivation.Order,
                        LastUpdMethodName = "SeedMotivations"
                    };
                    await _motivationService.AddMotivationAsync(newMotivation);
                    System.Diagnostics.Debug.WriteLine($"Motivation作成: {motivation.Name}");
                }
            }
        }

        /// <summary>
        /// データベース接続確認
        /// </summary>
        public async Task<bool> CanConnectToDatabaseAsync()
        {
            try
            {
                await _context.Database.CanConnectAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB接続エラー: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// MMファイルからHighTaskを自動生成する機能
        /// </summary>
        public async Task<int> AutoInsertMMFilesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== MMファイル自動insert開始 ===");
                
                var insertedCount = await _highTaskService.AutoInsertMMFilesAsync();
                
                System.Diagnostics.Debug.WriteLine($"MMファイルから{insertedCount}個のHighTaskを作成しました");
                System.Diagnostics.Debug.WriteLine("=== MMファイル自動insert完了 ===");
                
                return insertedCount;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== MMファイル自動insertエラー ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// MMファイルとHighTaskの同期機能（insert + delete）
        /// </summary>
        public async Task<(int inserted, int deleted)> SynchronizeMMFilesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== MMファイル同期開始 ===");
                
                var (insertedCount, deletedCount) = await _highTaskService.SynchronizeMMFilesAsync();
                
                System.Diagnostics.Debug.WriteLine($"MMファイル同期結果: {insertedCount}個追加, {deletedCount}個削除");
                System.Diagnostics.Debug.WriteLine("=== MMファイル同期完了 ===");
                
                return (insertedCount, deletedCount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== MMファイル同期エラー ===");
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}