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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WpfScheduledApp20250729.ViewModels;

namespace WpfScheduledApp20250729.Views
{
    /// <summary>
    /// ResultWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ResultWindow : Window
    {
        private ResultViewModel _viewModel;

        public ResultWindow()
        {
            InitializeComponent();
            Loaded += ResultWindow_Loaded;
        }

        public ResultWindow(ResultViewModel viewModel) : this()
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            
            // ViewModelのアニメーション完了イベントを購読
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ResultWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ウィンドウロード時に初期アニメーションを開始
            StartInitialAnimation();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.CurrentExp))
            {
                // EXP値が変更されたときのアニメーション
                if (_viewModel.CurrentExp == _viewModel.GainedExp && _viewModel.GainedExp > 0)
                {
                    // アニメーション完了時にフローティングテキストを表示
                    StartFloatingExpAnimation();
                }
            }
        }

        private void StartInitialAnimation()
        {
            // 画面全体のフェードインアニメーション
            var fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void StartFloatingExpAnimation()
        {
            // フローティングEXPテキストのアニメーション
            var floatingText = FindName("FloatingExpText") as TextBlock;
            if (floatingText != null)
            {
                // フローティングアニメーション開始
                var storyboard = FindResource("FloatingExpAnimation") as Storyboard;
                if (storyboard != null)
                {
                    //透明度を一時的に1に設定
                    floatingText.Opacity = 1;
                    
                    // Storyboard開始
                    Storyboard.SetTarget(storyboard, floatingText);
                    storyboard.Begin();
                }
            }

            // 追加効果音
            System.Media.SystemSounds.Exclamation.Play();
        }

        // ProgressBarの成長アニメーション
        public void StartProgressBarAnimation(double targetWidth)
        {
            var progressBar = FindName("ExpProgressBar") as Border;
            if (progressBar != null)
            {
                var widthAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = targetWidth,
                    Duration = TimeSpan.FromSeconds(2),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };

                progressBar.BeginAnimation(WidthProperty, widthAnimation);
            }
        }

        // XAMLで定義されたStoryboardを制御するためのヘルパーメソッド
        public void TriggerExpNumberAnimation(int targetValue)
        {
            var currentExpText = FindName("CurrentExpText") as TextBlock;
            if (currentExpText != null)
            {
                var storyboard = FindResource("ExpNumberAnimation") as Storyboard;
                if (storyboard != null)
                {
                    var animation = storyboard.Children[0] as DoubleAnimation;
                    if (animation != null)
                    {
                        animation.To = targetValue;
                        storyboard.Begin();
                    }
                }
            }
        }

        // ウィンドウを閉じる前のクリーンアップ
        protected override void OnClosed(EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            base.OnClosed(e);
        }
    }
}
