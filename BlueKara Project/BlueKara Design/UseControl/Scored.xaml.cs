using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for Scored.xaml
    /// </summary>
    public partial class Scored : UserControl
    {
        int score;
        int i = 1;
        DispatcherTimer t;
        string ContentScore = "";
        public Scored()
        {
            InitializeComponent();

            Random R = new Random();
            score = R.Next(70, 99);

            t = new DispatcherTimer();
            t.Tick += Displayscore;
            t.Interval = TimeSpan.FromMilliseconds(5);
            t.Start();



        }

        private void Displayscore(object sender, EventArgs e)
        {


            scorelabel.Content = i.ToString();
            i++;
            if (score >= 0 && score <= 70) ContentScore = "Bạn Hát Quá Tệ";
            else if (score >= 71 && score <= 79) ContentScore = "Bạn Hát Khá";
            else if (score >= 80 && score <= 89) ContentScore = "Bạn Hát Tốt";
            else if (score >= 90 && score <= 99) ContentScore = "Bạn Hát Rất Tốt";
            if (i == score)
            {
                t.Stop();
                t.Tick -= Displayscore;
                review.Content = ContentScore;
            }


        }
    }
}
