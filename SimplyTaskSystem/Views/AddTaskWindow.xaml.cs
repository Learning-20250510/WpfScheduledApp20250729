using SimplyTaskSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace SimplyTaskSystem.Views
{
    /// <summary>
    /// AddTaskWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
            Loaded += AddTaskWindow_Loaded;

            this.KMNTextBox.Focus();
        }

        private void AddTaskWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindows vm)
            {
                vm.Close += () =>
                {
                    this.Close();

                };
            }




        }
        private void AddTaskWindow_Closing(object sender, CancelEventArgs e)
        {
            AddTaskViewModel.StartedUp = false;

        }
    }
}
