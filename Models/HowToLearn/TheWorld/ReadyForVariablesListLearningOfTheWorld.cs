using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.TheWorld
{
    /// <summary>
    /// 変数リスト学習用の準備をする
    /// </summary>
    public class ReadyForVariablesListLearningOfTheWorld : IHowToLearnAction
    {
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public ReadyForVariablesListLearningOfTheWorld()
        {
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // 基本的なMMファイルを作成
            _freePlaneOperation.CreateBasicMMFile("Variables", "VariablesList", "Learning", 0);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            // 変数学習用MMファイルを作成
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName + "_Variables", taskId);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction(kmtName, kmn, htlName, taskId);
        }
    }
}