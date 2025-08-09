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

        public DataSeederService(
            BaseDbContext context,
            ArchitectureService architectureService,
            ProjectService projectService,
            HowToLearnService howToLearnService,
            PeriodicallyCycleService periodicallyCycleService)
        {
            _context = context;
            _architectureService = architectureService;
            _projectService = projectService;
            _howToLearnService = howToLearnService;
            _periodicallyCycleService = periodicallyCycleService;
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
            };

            foreach (var name in defaultArchitectures)
            {
                var existing = await _architectureService.GetArchitectureByNameAsync(name);
                if (existing == null)
                {
                    var newArchitecture = new Models.Entities.Architecture
                    {
                        ArchitectureName = name,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TouchedAt = DateTime.UtcNow,
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
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TouchedAt = DateTime.UtcNow,
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
                "Reading Documentation",
                "Online Tutorials",
                "Video Courses", 
                "Practice Projects",
                "Code Reviews",
                "Books",
                "Mentoring"
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
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TouchedAt = DateTime.UtcNow,
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
                "Daily",
                "Weekly",
                "Monthly", 
                "Quarterly",
                "Yearly",
                "As Needed"
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
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TouchedAt = DateTime.UtcNow,
                        LastUpdMethodName = "SeedPeriodicallyCycles"
                    };
                    await _periodicallyCycleService.AddAsync(newCycle);
                    System.Diagnostics.Debug.WriteLine($"PeriodicallyCycle作成: {name}");
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
    }
}