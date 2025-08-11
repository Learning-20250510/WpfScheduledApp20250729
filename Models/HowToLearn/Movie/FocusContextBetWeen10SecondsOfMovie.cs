using WpfScheduledApp20250729.Models.FilesOperation;

namespace WpfScheduledApp20250729.Models.HowToLearn.Movie
{
    /// <summary>
    /// 動画の10秒間の区間にフォーカスしてコンテキストを学習
    /// </summary>
    public class FocusContextBetWeen10SecondsOfMovie : IHowToLearnAction
    {
        private readonly string _movieFileName;
        private readonly int _startTime;
        private readonly MovieFileOperation _movieOperation;
        private readonly FreePlaneFileOperation _freePlaneOperation;

        public FocusContextBetWeen10SecondsOfMovie(string movieFileName, int startTime)
        {
            _movieFileName = movieFileName;
            _startTime = startTime;
            _movieOperation = new MovieFileOperation();
            _freePlaneOperation = new FreePlaneFileOperation();
        }

        public void TaskAction()
        {
            // 動画を指定時間から開く
            _movieOperation.OpenMovieFileWithSpecificTime(_movieFileName, _startTime);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId)
        {
            // 動画を指定時間から開く
            _movieOperation.OpenMovieFileWithSpecificTime(_movieFileName, _startTime);

            // MMファイルを作成
            _freePlaneOperation.CreateBasicMMFile(kmtName, kmn, htlName, taskId);
        }

        public void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2)
        {
            TaskAction(kmtName, kmn, htlName, taskId);
        }
    }
}