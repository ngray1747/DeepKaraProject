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

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount 
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void Goback(object sender, MouseButtonEventArgs e)
        {
            
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();
        }
    }
}
