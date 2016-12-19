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

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        public VideoPlayer()
        {
            InitializeComponent();
            
        }
       public void Changesize()
        {
            this.Width = 300;
            this.Height = 200;
            mediaElement.Height = 159;
            mediaElement.Width = 300;
            label.HorizontalAlignment = HorizontalAlignment.Right;
            label.Height = 50;
            label.Width = 300;
            label.FontSize = 25;
            label.Margin = new Thickness(0, 159, 0, 0);

        }
        public void Changesize2()
        {
            this.Width = 1350;
            this.Height = 693;
           mediaElement.Height = 630;
           mediaElement.Width = 1350;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Height = 63;
            label.Width = 570;
            label.FontSize = 30;
            label.Margin = new Thickness(780, 630, 0, 0);
          
            

        }
    }
}
