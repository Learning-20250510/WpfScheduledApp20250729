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

namespace SimplyTaskSystem.Views
{
    /// <summary>
    /// ResultWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ResultWindow : Window
    {
        public static bool StartedUp = false;

        public ResultWindow(bool isCleared)
        {
            InitializeComponent();
            Loaded += ResultWindow_Loaded;
            this.WindowState = WindowState.Maximized;
           

            if (isCleared == true)
            {
                this.Background = Brushes.Red;
            }
            else
            {

            }

        }

        private void ResultWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseWindows vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                };
            }


        }




    }
}
