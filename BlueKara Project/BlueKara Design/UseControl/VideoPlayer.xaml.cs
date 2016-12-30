using BlueKara_Design.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
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
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        DeepKaraEntities de = new DeepKaraEntities();
        WebClient web = new WebClient();
       public  DispatcherTimer D;
        public VideoPlayer()
        {
            InitializeComponent();

            D = new DispatcherTimer();
            D.Interval = TimeSpan.FromSeconds(1);
            D.Tick += MoreTime;
           


        }



        public void Changesize()
        {
            this.Width = 300;
            this.Height = 200;
            mediaElement.Height = 159;
            mediaElement.Width = 300;
            lbtenbaihat.HorizontalAlignment = HorizontalAlignment.Right;
            lbtenbaihat.Height = 50;
            lbtenbaihat.Width = 300;
            lbtenbaihat.FontSize = 25;
            lbtenbaihat.Margin = new Thickness(0, 159, 0, 0);

        }
        public void Changesize2()
        {
            this.Width = 1350;
            this.Height = 693;
            mediaElement.Height = 630;
            mediaElement.Width = 1350;
            lbtenbaihat.HorizontalAlignment = HorizontalAlignment.Left;
            lbtenbaihat.VerticalAlignment = VerticalAlignment.Top;
            lbtenbaihat.Height = 63;
            lbtenbaihat.Width = 570;
            lbtenbaihat.FontSize = 30;
            lbtenbaihat.Margin = new Thickness(780, 630, 0, 0);

        }


        private void StopVideo(object sender, MouseButtonEventArgs e)
        {

            string source = mediaElement.Source.AbsolutePath;

            source = source.Replace("%20", " ");

            //xóa file trên ổ cứng


            mediaElement.Stop();

            mediaElement.Close();
            Thread.Sleep(2000);

            //video online tải về mới xóa
            if (source.StartsWith("C"))
            {
                File.Delete(source);
            }

            Usecontrolvideoplayer.Visibility = Visibility.Hidden;



        }

        private void PauseVideo(object sender, MouseButtonEventArgs e)
        {
            mediaElement.Pause();
            Pause.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;

        }

        private void PlayVideo(object sender, MouseButtonEventArgs e)
        {

            mediaElement.Play();
            Pause.Visibility = Visibility.Visible;
            Play.Visibility = Visibility.Hidden;
        }


        bool isDragging = false;
        public void seekBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;

        }

        public void seekBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            mediaElement.Position = TimeSpan.FromSeconds(slider.Value);
        }
        public void MoreTime(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                slider.Value = mediaElement.Position.TotalSeconds;



            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan a = new TimeSpan(0, 0, Convert.ToInt32(mediaElement.Position.TotalSeconds));
            tblTimePass.Text = a.ToString();
        }

        private void OpenSong(object sender, RoutedEventArgs e)
        {
            D.Start();
            mediaElement.Play();
            slider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            TimeSpan b = new TimeSpan(0, 0, Convert.ToInt32(mediaElement.NaturalDuration.TimeSpan.TotalSeconds));
            tblTimeTotal.Text = b.ToString();
        }

        
    }

}
