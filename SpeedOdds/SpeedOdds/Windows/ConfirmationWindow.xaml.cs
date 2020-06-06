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

namespace SpeedOdds.Windows
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow(string msg)
        {
            InitializeComponent();

            if (!String.IsNullOrWhiteSpace(msg))
                TextBlockMsg.Text = msg;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Back)
            {
                this.DialogResult = false;
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
