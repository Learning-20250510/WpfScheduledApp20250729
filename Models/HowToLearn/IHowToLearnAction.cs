namespace WpfScheduledApp20250729.Models.HowToLearn
{
    public interface IHowToLearnAction
    {
        void TaskAction();
        void TaskAction(string kmtName, string kmn, string htlName, int taskId);
        void TaskAction(string kmtName, string kmn, string htlName, int taskId, string relationalFile1, string relationalFile2);
    }
}