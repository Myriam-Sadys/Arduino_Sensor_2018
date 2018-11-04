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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Globalization;
using Sparrow.Chart;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BorderCharts.Opacity == 1)
            {
                Grid.SetRowSpan(Charts, 2);
                Grid.SetColumnSpan(Charts, 3);
                BorderCharts.Opacity = 0;
                Background.Opacity = 0.2;
            }
            else
            {
                Grid.SetRowSpan(Charts, 1);
                Grid.SetColumnSpan(Charts, 1);
                BorderCharts.Opacity = 1;
                Background.Opacity = 0.9;
            }
        }
    }
}
