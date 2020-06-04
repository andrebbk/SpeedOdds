using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeedOdds.Commons.Helpers
{
    public static class UtilsNotification
    {
        //Loading Animation
        public static LottieSharp.LottieAnimationView LoadingLottieAnimationView { get; set; }
        public static TextBlock TextBoxLoading { get; set; }
        public static UserControl_Menu MenuController { get; set; }
        public static UserControl_ControlBox ControlBoxController { get; set; }

        public static void StartLoadingAnimation()
        {
            new Thread(() =>
            {
                LoadingLottieAnimationView.Dispatcher.BeginInvoke((Action)(() => LoadingLottieAnimationView.PlayAnimation()));

                LoadingLottieAnimationView.Dispatcher.BeginInvoke((Action)(() => LoadingLottieAnimationView.Visibility = System.Windows.Visibility.Visible));

                TextBoxLoading.Dispatcher.BeginInvoke((Action)(() => TextBoxLoading.Visibility = System.Windows.Visibility.Visible));

                MenuController.LockMenu();
                ControlBoxController.LockControlBox();

            }).Start();
        }

        public static void StopLoadingAnimation()
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);

                LoadingLottieAnimationView.Dispatcher.BeginInvoke((Action)(() => LoadingLottieAnimationView.PauseAnimation()));

                LoadingLottieAnimationView.Dispatcher.BeginInvoke((Action)(() => LoadingLottieAnimationView.Visibility = System.Windows.Visibility.Hidden));

                TextBoxLoading.Dispatcher.BeginInvoke((Action)(() => TextBoxLoading.Visibility = System.Windows.Visibility.Hidden));

                MenuController.UnlockMenu();
                ControlBoxController.UnlockControlBox();

            }).Start();
        }
    }
}
