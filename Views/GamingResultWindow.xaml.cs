using System.Windows;
using WpfScheduledApp20250729.ViewModels;

namespace WpfScheduledApp20250729.Views
{
    public partial class GamingResultWindow : Window
    {
        public GamingResultWindow()
        {
            InitializeComponent();
        }

        public GamingResultWindow(GamingResultViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}