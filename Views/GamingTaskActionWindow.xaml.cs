using System.Windows;
using WpfScheduledApp20250729.ViewModels;

namespace WpfScheduledApp20250729.Views
{
    public partial class GamingTaskActionWindow : Window
    {
        public GamingTaskActionWindow()
        {
            InitializeComponent();
        }

        public GamingTaskActionWindow(TaskActionViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is TaskActionViewModel viewModel)
            {
                viewModel.Dispose();
            }
        }
    }
}