using SpeedOdds.Windows;
using System;
using System.Threading;
using System.Windows.Controls;


namespace SpeedOdds.UserControls.Start
{
    /// <summary>
    /// Interaction logic for UserControl_StartWindow.xaml
    /// </summary>
    public partial class UserControl_StartWindow : UserControl
    {
        public UserControl_StartWindow(StartWindow startWindow)
        {
            InitializeComponent();

            new Thread(() =>
            {
                Thread.Sleep(2000);

                LabelWelcome.Dispatcher.BeginInvoke((Action)(
                    () => LabelWelcome.Visibility = System.Windows.Visibility.Visible
                ));

                Thread.Sleep(1000);

                startWindow.Dispatcher.BeginInvoke((Action)(
                  () => new MainWindow(startWindow).Show()
                ));

            }).Start();
        }
    }
}
