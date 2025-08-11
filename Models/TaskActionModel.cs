using System;

namespace WpfScheduledApp20250729.Models
{
    public class TaskActionModel
    {
        public int ID { get; set; }
        public string KMN { get; set; } = string.Empty;
        public string KMT { get; set; } = string.Empty;
        public int HTL { get; set; }
        public int EstimatedTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ScrollValue { get; set; }
        public string RelationalFile1 { get; set; } = string.Empty;
        public string RelationalFile2 { get; set; } = string.Empty;
        
        // 入力フィールド
        public int VoiceMemo { get; set; }
        public int ManualMemoBySP { get; set; }
        public int ManualMemoNumberOfPagesByAnalog { get; set; }
        public int ConcentrateTime { get; set; }
        public string AutoCreateMMFileGenerator { get; set; } = string.Empty;
        
        // 計算されるフィールド
        public int DamageCount { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        
        // HTL関連
        public string HTLName { get; set; } = string.Empty;
        public string KMTName { get; set; } = string.Empty;
        public int MovieAndSoundStartTimeOfTen { get; set; }
        public int MovieStartTimeOfTwo { get; set; }
        public int SpecificPageOfPDF { get; set; }
        public double ValueOfScrollOfWebPage { get; set; }
    }
}