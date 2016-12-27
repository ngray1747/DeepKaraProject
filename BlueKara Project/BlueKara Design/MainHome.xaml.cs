using BlueKara_Design;
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
using System.Windows.Interop;
using WPF.MDI;
using BlueKara_Design.UseControl;
using Facebook;
using BlueKara_Design.Database;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using System.IO;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for MainHome.xaml
    /// </summary>
    public partial class MainHome
    {
        string filename;
        DeepKaraEntities de = new DeepKaraEntities();

        string _playerID;
        public MainHome(string playerID)
        {
            InitializeComponent();
            //ShowYouTubeVideo(web, "");
            //web2.Navigate("https://www.youtube.com/watch?v=4T1cggtX8XY");
            //AreaControl.Children.Add(new MdiChild()
            //{
            //    Title = "VideoPlayer",
            //    Height = 300,
            //    Width = 300,

            //    Content = new VideoPlayer()
            //});
            _playerID = playerID;

            AppearInfo();
            GetListKaraoke();



        }

        private async void GetListKaraoke()
        {
            ChangeProgressRing();
            var c = (dynamic)null;
            await Task.Run(() =>
             {

                 c = (from a in de.CHUNG_VIDEOKARAOKE
                      join b in de.VIDEOONLINEs on a.ID equals b.VideoID
                      select new ListKaraoke
                      {
                          maso = b.VideoID,
                          tenbaihat = a.Name,
                          casi = b.SINGER.FullName,
                          ngaycapnhat = b.DateUpdate.Value,
                          nhacsi = b.COMPOSER.FullName,
                          theloai = a.CATEGORY.Name

                      }).ToList();

             });
            gridlistMusic.ItemsSource = c;
            ChangeProgressRingagain();

        }

        private async void AppearInfo()
        {
            ChangeProgressRing();
            var pl = (dynamic)null;
            await Task.Run(() =>
            {
                pl = (from a in de.PLAYERs
                      where a.PlayerID == _playerID
                      join b in de.LEVELs on a.PlayerID equals b.PlayerID
                      select new
                      {
                          a.Username,
                          b.MaxScore,
                          b.Name,
                          b.RemainScoreToUp,
                      }).FirstOrDefault();


            });

            lbUsername.Content = pl.Username;
            lbLevel.Content = pl.Name;
            txbCurentscore.Text = (pl.MaxScore - pl.RemainScoreToUp).ToString() + "    / ";
            txbMaxscore.Text = pl.MaxScore.ToString();
            progressScore.Value = pl.MaxScore - pl.RemainScoreToUp;
            progressScore.Maximum = pl.MaxScore;
            ChangeProgressRingagain();

        }

        public MainHome()
        {
            InitializeComponent();
            //ShowYouTubeVideo(web, "");
            //web2.Navigate("https://www.youtube.com/watch?v=4T1cggtX8XY");
            //AreaControl.Children.Add(new MdiChild()
            //{
            //    Title = "VideoPlayer",
            //    Height = 300,
            //    Width = 300,

            //    Content = new VideoPlayer()
            //});

            videoplayer.Visibility = Visibility.Visible;
            videoplayer.mediaElement.Play();

        }
        static int cout = 0;

        private void ChangeSizeWindow(object sender, MouseButtonEventArgs e)
        {
            if (cout % 2 == 0)
            {
                videoplayer.Margin = new Thickness(908, 410, 142, 0);
                Grid.SetRow(videoplayer, 1);
                videoplayer.Changesize();
            }
            else
            {
                videoplayer.Margin = new Thickness(0, -96, 0, 0);
                Grid.SetRow(videoplayer, 2);
                videoplayer.Changesize2();
            }
            cout++;



        }

        private static string GetYouTubeVideoPlayerHTML(string videoCode)
        {
            var sb = new StringBuilder();

            const string YOUTUBE_URL = @"https://www.youtube.com/v/";

            sb.Append("<html>");
            sb.Append("    <head>");
            sb.Append("        <meta name=\"viewport\" content=\"width=device-width; height=device-height;\">");
            sb.Append("    </head>");
            sb.Append("    <body marginheight=\"0\" marginwidth=\"0\" leftmargin=\"0\" topmargin=\"0\" style=\"overflow-y: hidden\">");
            sb.Append("        <object width=\"100%\" height=\"100%\">");
            sb.Append("            <param name=\"movie\" value=\"" + YOUTUBE_URL + videoCode + "?version=3&amp;rel=0\" />");
            sb.Append("            <param name=\"allowFullScreen\" value=\"true\" />");
            sb.Append("            <param name=\"allowscriptaccess\" value=\"always\" />");
            sb.Append("            <embed src=\"" + YOUTUBE_URL + "?version=3&amp;rel=0\" type=\"application/x-shockwave-flash\"");
            sb.Append("                   width=\"100%\" height=\"100%\" allowscriptaccess=\"always\" allowfullscreen=\"true\" />");
            sb.Append("        </object>");
            sb.Append("    </body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        public static void ShowYouTubeVideo(WebBrowser webBrowser, string videoCode)
        {
            if (webBrowser == null) throw new ArgumentNullException("webBrowser");

            webBrowser.NavigateToString(GetYouTubeVideoPlayerHTML(videoCode));
        }



        private void CloseFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutInfo.IsOpen = false;
            closefyout.Visibility = Visibility.Hidden;
        }

        private async void OpenFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutInfo.IsOpen = true;
            closefyout.Visibility = Visibility.Visible;
        }

        private async void ChangeInfo(object sender, RoutedEventArgs e)
        {
            ChangeProgressRing();
            var pl = (dynamic)null;
            await Task.Run(() =>
            {
                pl = (from m in de.PLAYERs
                      where m.PlayerID == _playerID
                      select m).FirstOrDefault();
            });
            pl.Email = txbEmail.Text;
            pl.FacebookURL = txbLink.Text;
            pl.Job = txbJob.Text;
            if (txbDate.Text != pl.BirthDate.ToString())
            {
                pl.BirthDate = pickdate.SelectedDate.Value;

            }

            pl.FullName = txbName.Text;
            pl.PhoneNumber = txbPhone.Text;



            if (pl.Password == psCurrentPass.Password && pwPassWord.Password == psnewpass.Password)
            {
                pl.Password = psnewpass.Password;
                de.SaveChanges();
                ChangeProgressRingagain();
                return;
            }



            await this.ShowMessageAsync("Thông Báo", "Bạn đã nhập sai mật khẩu. Vui lòng nhập lại", MessageDialogStyle.Affirmative);
            ChangeProgressRingagain();


        }

        private void UpdateInfo(object sender, RoutedEventArgs e)
        {
            var k = (from a in de.PLAYERs
                     join b in de.LEVELs on a.PlayerID equals b.PlayerID
                     where a.PlayerID == _playerID
                     select new
                     {
                         a.Email,
                         a.Username,
                         b.Name,
                         b.MaxScore,
                         b.RemainScoreToUp,
                         a.PhoneNumber,
                         a.Job,
                         a.FacebookURL,
                         a.BirthDate,
                         a.FullName,
                         a.Password

                     }).FirstOrDefault();
            try
            {
                txbEmail.Text = k.Email.ToString();
                txbUsername.Text = k.Username;
                txbLevel.Text = k.Name.Split(' ')[1];
                txbScore.Text = (k.MaxScore - k.RemainScoreToUp).ToString();
                txbLink.Text = k.FacebookURL;
                txbJob.Text = k.Job;
                txbDate.Text = k.BirthDate.ToString();
                txbPhone.Text = k.PhoneNumber;


            }
            catch { }

        }

        private void ChangeProgressRingagain()
        {
            Progressring.Visibility = Visibility.Hidden;
            GifBackground.Visibility = Visibility.Hidden;
            mainGrid.IsEnabled = true;
        }

        private void ChangeProgressRing()
        {
            Progressring.Visibility = Visibility.Visible;
            GifBackground.Visibility = Visibility.Visible;
            mainGrid.IsEnabled = false;

        }

        private void onlynumer(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            if (await this.ShowMessageAsync("Thông Báo", "Bạn có muốn đăng xuất?", MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
            {
                MainWindow a = new MainWindow();
                a.Show();
                this.Close();

            }
            else { return; }
        }

        private async void OpenSong(object sender, MouseButtonEventArgs e)
        {
            object item = gridlistMusic.SelectedItem;
            string ID = (gridlistMusic.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;

            var k = (from a in de.CHUNG_VIDEOKARAOKE
                     where a.ID == ID
                     select new
                     {
                         a.VIDEOONLINE.URLVideo,
                         a.Name
                     }).FirstOrDefault();
            filename = k.URLVideo;
            Task<int> task = new Task<int>(DowloadFromFTP);
            task.Start();
            ChangeProgressRing();
            await task;

            ChangeProgressRingagain();

            string inputvideo = string.Format("C:\\ShareFolderMusic\\{0}", filename);
            tblTenbaihat.Text = k.Name;
            tblWelcome.Text = "Bạn đang hát ca khúc ";

            PlayVideo(inputvideo,k.Name);



        }

        private void PlayVideo(string inputvideo,string tenbaihat)
        {
            videoplayer.Visibility = Visibility.Visible;
            videoplayer.mediaElement.Source = new Uri(inputvideo);
            videoplayer.mediaElement.Play();
            videoplayer.lbtenbaihat.Text = tenbaihat;
        }

        private int DowloadFromFTP()
        {

            string inputfilepath = String.Format("C:\\ShareFolderMusic\\{0}", filename);

            string ftphost = "123.31.34.57";
            string ftpfilepath = String.Format("/{0}", filename);

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

            return 0;
        }
    }



    public class NegatingConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return -((double)value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return +(double)value;
            }
            return value;
        }
    }
}
