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

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        DeepKaraEntities de = new DeepKaraEntities();
        WebClient web = new WebClient();

        public VideoPlayer()
        {
            InitializeComponent();

            //DownloadFileFTP();

            //mediaElement.Source = new Uri("C:\\ShareFolderMusic\\Biet anh sai.avi");
            //mediaElement.Play();



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
        private void DownloadFileFTP()
        {
            string inputfilepath = @"C:\ShareFolderMusic\Biet anh sai.avi";
            string ftphost = "123.31.34.57";
            string ftpfilepath = "/Biet anh sai.avi";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("Administrator", "Tietkhaiky2");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }

            }
        }

        private void StopVideo(object sender, MouseButtonEventArgs e)
        {

            string source = mediaElement.Source.AbsolutePath;

            source = source.Replace("%20", " ");
           
            //xóa file trên ổ cứng
            

            mediaElement.Stop();
            
            mediaElement.Close();
            Thread.Sleep(2000);
            File.Delete(source);
            
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

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }

}
