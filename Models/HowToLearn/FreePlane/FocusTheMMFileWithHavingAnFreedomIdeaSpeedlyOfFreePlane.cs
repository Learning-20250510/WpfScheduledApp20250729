using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.FreePlane
{
    /// <summary>
    /// FreePlaneのMMファイルを開いて、自由な発想でスピーディーに思考する
    /// </summary>
    public class FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane : IHowToLearnAction
    {
        private readonly string _mmFileName;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane(string mmFileName)
        {
            _mmFileName = mmFileName;
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // MMファイルを開く（速い思考モード）
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