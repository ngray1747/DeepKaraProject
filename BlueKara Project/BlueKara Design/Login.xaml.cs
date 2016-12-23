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

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LinkSpecial(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Blue;
            (sender as TextBlock).FontStyle = FontStyles.Italic;
            (sender as TextBlock).TextDecorations = TextDecorations.Underline;

        }

        private void LinkSpecial2(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Black;
            (sender as TextBlock).FontStyle = FontStyles.Oblique;
            (sender as TextBlock).TextDecorations = null;

        }

        private void Gointoforgetpass(object sender, MouseButtonEventArgs e)
        {
            ResetPass a = new ResetPass();
            a.Show();
            this.Close();
        }

        private void Gointcreate(object sender, MouseButtonEventArgs e)
        {
            CreateAccount a = new CreateAccount();
            a.Show();
            this.Close();
        }
    }
}
