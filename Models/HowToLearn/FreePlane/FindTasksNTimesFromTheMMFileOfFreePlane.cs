using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.FreePlane
{
    /// <summary>
    /// FreePlaneのMMファイルからN回分のタスクを見つける
    /// </summary>
    public class FindTasksNTimesFromTheMMFileOfFreePlane : IHowToLearnAction
    {
        private readonly string _mmFileName;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public FindTasksNTimesFromTheMMFileOfFreePlane(string mmFileName)
        {
            _mmFileName = mmFileName;
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // MMファイルを開いてタスクを探す
            _freePlaneOperation.OpenSpecificFreePlaneFile(_mmFileName);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            TaskAction();
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction();
        }
    }
}