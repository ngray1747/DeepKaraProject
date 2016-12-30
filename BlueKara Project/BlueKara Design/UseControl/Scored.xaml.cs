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
using BlueKara_Design.Database;

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for Scored.xaml
    /// </summary>
    public partial class Scored : UserControl
    {
        DeepKaraEntities de = new DeepKaraEntities();

        string _playerID;
        int score;
        DispatcherTimer scoreRun = new DispatcherTimer();
        string ContentScore = "";
        int i = 0;
        public Scored()
        {
            InitializeComponent();


        }
        public void ChamDiem(string playerid)
        {
            _playerID = playerid;

            Random R = new Random();
            score = R.Next(70, 99);
            scoreRun.Tick += new EventHandler(Displayscore);
            scoreRun.Interval += TimeSpan.FromMilliseconds(5);
            scoreRun.Start();



        }
        public void Displayscore(object sender, EventArgs e)
        {
            i++;
            scorelabel.Content = i.ToString();

            if (i == score)
            {
                if (score >= 71 && score <= 79) ContentScore = "Bạn Hát Khá";
                else if (score >= 80 && score <= 89) ContentScore = "Bạn Hát Tốt";
                else if (score >= 90 && score <= 99) ContentScore = "Bạn Hát Rất Tốt";

                scoreRun.Stop();
                scoreRun.IsEnabled = false;

                scorelabel.Content = score.ToString();
                review.Content = ContentScore;
                CongDiemvaLevel();
                PlayNhac();
            }
        }

        private void PlayNhac()
        {

            mediaVotay.Source = new Uri("../../Image/voaty.mp3", UriKind.Relative);

            mediaVotay.LoadedBehavior = MediaState.Manual;
            mediaVotay.UnloadedBehavior = MediaState.Stop;
            mediaVotay.Play();

        }

        private void CongDiemvaLevel()
        {
            var k = (from a in de.LEVELs
                     where a.PlayerID == _playerID
                     select a).FirstOrDefault();
            k.PLAYER.Scores = score;
            k.RemainScoreToUp -= score;

            if (k.RemainScoreToUp <= 0)
            {
                string curentLevel = k.Name;
                int curentMaxscore = (int)k.MaxScore;
                k.Name = "Level " + (int.Parse((curentLevel.Split(' ')[1])) + 1).ToString();
                k.MaxScore = curentMaxscore + 500;//lên 1 level tăng điểm max lên 500
                k.RemainScoreToUp = curentMaxscore + 500;


            }

            de.SaveChanges();

        }
    }
}
