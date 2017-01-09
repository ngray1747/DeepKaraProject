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
using BlueKara_Design.UseControl;
using Facebook;
using BlueKara_Design.Database;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.MappingAPI;
using NAudio.Wave;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for MainHome.xaml
    /// </summary>
    public partial class MainHome
    {
        Scored sc;
        string filename;
        DeepKaraEntities de = new DeepKaraEntities();
        ObservableCollection<ListKaraokeQueue> listQueue = new ObservableCollection<ListKaraokeQueue>();
        ObservableCollection<ListKaraoke> listCurrent = new ObservableCollection<ListKaraoke>();
        public string _playerID;

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



            videoplayer.Next.MouseLeftButtonUp += new MouseButtonEventHandler(NextInQueue);


            _playerID = playerID;

            AppearInfo();
            GetListKaraoke();
        }

        private async void NextInQueue(object sender, MouseButtonEventArgs e)
        {
            try
            {
                videoplayer.mediaElement.MediaEnded -= EndedVideo;
            }
            catch { }

            if (SwitchType.IsChecked == true)
            {
                try
                {
                    var theFirstVideo = listQueue.FirstOrDefault();

                    Thread.Sleep(1000);

                    DeleteFile();

                    videoplayer.Visibility = Visibility.Hidden;

                    string Name = "";

                    GetInfoVideoOnline(theFirstVideo._maso, ref Name, ref filename);
                    //cần url video,Name

                    Task<int> task = new Task<int>(DowloadFromFTP);
                    task.Start();
                    ChangeProgressRing();
                    await task;
                    ChangeProgressRingagain();

                    string inputvideo = string.Format("C:\\ShareFolderMusic\\{0}", filename);
                    tblTenbaihat.Text = Name;
                    tblWelcome.Text = "Bạn đang hát ca khúc ";

                    PlayVideo(inputvideo, Name);

                    //remove danh sách queue
                    RemoveKaraokeFromQueue_(theFirstVideo._maso);

                }
                catch { }
            }
            else
            {
                var theFirstVideo = listQueue.FirstOrDefault();

                Thread.Sleep(1000);

                videoplayer.Visibility = Visibility.Hidden;

                var k = (from a in listQueue
                         where a._maso == theFirstVideo._maso
                         join b in listCurrent on a._maso equals b.maso
                         select new { a._tenbaihat, a._maso, b.url }).FirstOrDefault();
                if (k==null) return;
                string Name = k._tenbaihat;
                string inputvideo = k.url;
                PlayVideo(inputvideo, Name);

                RemoveKaraokeFromQueue_(theFirstVideo._maso);


            }
        }

        private async void GetListKaraoke()
        {
            listCurrent.Clear();
            if (SwitchType.IsChecked == true)//online type
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
                foreach (var a in c)
                {
                    listCurrent.Add(a);
                }

                gridlistMusic.ItemsSource = listCurrent;
                ChangeProgressRingagain();

            }
            else //offline type
            {

                OpenFileDialog file = new OpenFileDialog();
                file.DefaultExt = ".";
                file.Multiselect = true;
                file.Filter = "Image File (*.avi,*.mp4,*.mkv,*.wmv,*.vob,*.flv,*.3pg,*.mov,*.mpg) | *.avi;*.mp4;*.mkv;*.wmv;*.vob;*.flv;*.3gp;*.mov;*.mpg";

                var soluongbaihat = from a in de.CHUNG_VIDEOKARAOKE
                                    select a;
                int i = soluongbaihat.Count() + 1;
                string ID = "";

                if (file.ShowDialog() == true)
                {
                    foreach (var s in file.FileNames)
                    {

                        FileInfo fileinfo = new FileInfo(s);
                        //add vô list
                        //tạo ID
                        if (i.ToString().Length == 1)
                        {
                            ID = "VD0000" + i.ToString();
                        }
                        else if (i.ToString().Length == 2)
                        {
                            ID = "VD000" + i.ToString();
                        }
                        else if (i.ToString().Length == 3)
                        {
                            ID = "VD00" + i.ToString();
                        }
                        else if (i.ToString().Length == 4)
                        {
                            ID = "VD0" + i.ToString();
                        }
                        else if (i.ToString().Length == 5)
                        {
                            ID = "VD" + i.ToString();
                        }


                        listCurrent.Add(new ListKaraoke() { maso = ID, tenbaihat = fileinfo.Name.Split('.')[0], url = fileinfo.FullName, ngaycapnhat = fileinfo.LastWriteTime, format = fileinfo.Extension, theloai = "Unknow", casi = "Unknow", nhacsi = "Unknow" });
                        i++;

                    }
                }


                gridlistMusic.ItemsSource = listCurrent;
                gridlistMusic.Items.Refresh();
                ChangeProgressRing();

                await Task.Run(() =>
                {
                    //cập nhật vô csdl 3 bảng: chung_videokaraoke và videooffline và videokaraoke
                    foreach (ListKaraoke a in listCurrent)
                    {
                        CHUNG_VIDEOKARAOKE chung = new CHUNG_VIDEOKARAOKE();
                        chung.ID = a.maso;
                        chung.Name = a.tenbaihat;
                        chung.Duaration = 0;
                        chung.CategoryID = "CT07";//unknow
                        switch (a.format.ToLower())
                        {
                            case "avi":
                                chung.FormatVideoID = "FM01";
                                break;
                            case "mp4":
                                chung.FormatVideoID = "FM02";
                                break;
                            case "mkv":
                                chung.FormatVideoID = "FM03";
                                break;
                            case "mpg":
                                chung.FormatVideoID = "FM04";
                                break;
                            case "wmv":
                                chung.FormatVideoID = "FM05";
                                break;
                            case "vob":
                                chung.FormatVideoID = "FM06";
                                break;
                            case "flv":
                                chung.FormatVideoID = "FM07";
                                break;
                            case "mov":
                                chung.FormatVideoID = "FM08";
                                break;
                            case "3gp":
                                chung.FormatVideoID = "FM09";
                                break;
                        }
                        de.CHUNG_VIDEOKARAOKE.Add(chung);
                        VIDEOKARAOKE videokaraoke = new VIDEOKARAOKE();
                        videokaraoke.VideoID = a.maso;
                        de.VIDEOKARAOKEs.Add(videokaraoke);
                        VIDEOOFFLINE videoOffline = new VIDEOOFFLINE();
                        videoOffline.VideoID = a.maso;
                        videoOffline.LinkVideo = a.url;
                        de.VIDEOOFFLINEs.Add(videoOffline);

                    }

                    de.Configuration.AutoDetectChangesEnabled = false;
                    de.Configuration.ValidateOnSaveEnabled = false;
                    de.SaveChanges();
                });
                ChangeProgressRingagain();

            }
        }


        private void AppearInfo()
        {

            var pl = (from a in de.PLAYERs
                      where a.PlayerID == _playerID
                      join b in de.LEVELs on a.PlayerID equals b.PlayerID
                      select new
                      {
                          a.Username,
                          b.MaxScore,
                          b.Name,
                          b.RemainScoreToUp,
                      }).FirstOrDefault();


            lbUsername.Content = pl.Username;
            lbLevel.Content = pl.Name;
            txbCurentscore.Text = (pl.MaxScore - pl.RemainScoreToUp).ToString() + "    / ";
            txbMaxscore.Text = pl.MaxScore.ToString();
            progressScore.Value = Convert.ToDouble(pl.MaxScore - pl.RemainScoreToUp);
            progressScore.Maximum = Convert.ToDouble(pl.MaxScore);

            //try
            //{
            //    mainGrid.Children.Remove(sc);
            //}
            //catch { }
        }


        static int cout = 0;



        private void ChangeSizeWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (cout % 2 == 0)
                {
                    ChangeSizeMinimum();
                }
                else
                {
                    ChangeSizeMaximum();
                }
                cout++;
            }
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

        private void OpenFlyout(object sender, RoutedEventArgs e)
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

            if (e.ChangedButton == MouseButton.Left)
            {


                object item = gridlistMusic.SelectedItem;
                string ID = (gridlistMusic.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                string Name = "";
                string inputvideo = "";//url video input
                if (SwitchType.IsChecked == true)
                {
                    GetInfoVideoOnline(ID, ref Name, ref filename);
                    //cần url video,Name

                    Task<int> task = new Task<int>(DowloadFromFTP);
                    task.Start();
                    ChangeProgressRing();
                    await task;
                    ChangeProgressRingagain();

                    inputvideo = string.Format("C:\\ShareFolderMusic\\{0}", filename);

                }
                else
                {
                    var k = (from a in listCurrent
                             where a.maso == ID
                             select a).FirstOrDefault();
                    Name = k.tenbaihat;
                    inputvideo = k.url;

                }
                tblTenbaihat.Text = Name;

                tblWelcome.Text = "Bạn đang hát ca khúc ";
                PlayVideo(inputvideo, Name);

            }

        }



        private void GetInfoVideoOnline(string ID, ref string name, ref string URL)
        {
            var k = (from a in de.CHUNG_VIDEOKARAOKE
                     where a.ID == ID
                     select new
                     {
                         a.VIDEOONLINE.URLVideo,
                         a.Name
                     }).FirstOrDefault();
            name = k.Name;
            URL = k.URLVideo;
        }

        private void PlayVideo(string inputvideo, string tenbaihat)
        {

            videoplayer.Visibility = Visibility.Visible;
            videoplayer.mediaElement.Source = new Uri(inputvideo);
            ChangeSizeMaximum();

            videoplayer.mediaElement.Play();

            videoplayer.lbtenbaihat.Text = tenbaihat;
            videoplayer.mediaElement.MediaEnded += new RoutedEventHandler(EndedVideo);


        }

        private void ChangeSizeMinimum()
        {
            videoplayer.Margin = new Thickness(908, 410, 142, 0);
            Grid.SetRow(videoplayer, 1);
            videoplayer.Changesize();

        }
        private void ChangeSizeMaximum()
        {
            videoplayer.Margin = new Thickness(0, -96, 0, 0);
            Grid.SetRow(videoplayer, 2);
            videoplayer.Changesize2();

        }

        private void EndedVideo(object sender, RoutedEventArgs e)
        {

            videoplayer.mediaElement.Close();
            Thread.Sleep(500);
            DeleteFile();

            videoplayer.Visibility = Visibility.Hidden;

            AddControlScore();




            try
            {

                videoplayer.mediaElement.MediaEnded -= EndedVideo;

            }
            catch
            {

            }

        }

        private void AddControlScore()
        {

            sc = new Scored();
            Grid.SetRow(sc, 2);
            sc.Margin = new Thickness(0, 0, 0, -7);


            mainGrid.Children.Add(sc);
            sc.ChamDiem(_playerID);


            sc.mediaVotay.MediaEnded += new RoutedEventHandler(EndVotay);


            // mainGrid.Children.Remove(sc);



        }

        private void EndVotay(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Remove(sc);
            sc = null;

            ////cập nhật lại điểm và level


            AppearInfo();
            UpdateInfo(null, null);
            try
            {
                sc.mediaVotay.MediaEnded -= EndVotay;
            }
            catch
            {

            }

            videoplayer.mediaElement.Close();

            NextInQueue(null, null);
        }

        private void DeleteFile()
        {
            string source = videoplayer.mediaElement.Source.AbsolutePath;

            source = source.Replace("%20", " ");

            //xóa file trên ổ cứng nếu là video online
            videoplayer.mediaElement.Close();
            if (source.StartsWith("C"))
            {
                Thread.Sleep(2000);
                File.Delete(source);
            }
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



        private void AddKaraoketoQueue(object sender, RoutedEventArgs e)
        {

            object item = gridlistMusic.SelectedItem;
            string ID = (gridlistMusic.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            try
            {
                var k = (from a in de.CHUNG_VIDEOKARAOKE
                         where a.ID == ID
                         select new ListKaraokeQueue
                         {
                             _maso = a.ID,
                             _theloai = a.CATEGORY.Name,
                             _tenbaihat = a.Name
                         }).FirstOrDefault();

                listQueue.Add(k as ListKaraokeQueue);
            }
            catch { }
            gridQueue.ItemsSource = listQueue;


        }

        private void RemoveKaraokeFromQueue(object sender, RoutedEventArgs e)
        {

            object item = gridQueue.SelectedItem;
            string ID = (gridQueue.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            RemoveKaraokeFromQueue_(ID);

        }

        private void RemoveKaraokeFromQueue_(string iD)
        {
            try
            {
                var k = (from a in listQueue
                         where a._maso == iD
                         select a).FirstOrDefault();

                listQueue.Remove(k as ListKaraokeQueue);
            }
            catch { }
            gridQueue.ItemsSource = listQueue;
        }

        private async void EnterSearch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (tbKeySearch.Text == "")
                {
                    gridlistMusic.ItemsSource = listCurrent;
                }

            }
            else
            {
                ObservableCollection<ListKaraoke> listResult = new ObservableCollection<ListKaraoke>();

                if (e.Key == Key.Enter)
                {
                    string keyWord1 = tbKeySearch.Text;
                    string typeOfSearch = ((ComboBoxItem)(cbboxType.SelectedItem)).Content.ToString();
                    string keyWordUnsign = convertToUnSign(keyWord1);

                    var k = (from a in listCurrent
                             select new ListKaraoke
                             {
                                 maso = a.maso,
                                 tenbaihat = a.tenbaihat,
                                 casi = a.casi,
                                 ngaycapnhat = a.ngaycapnhat,
                                 nhacsi = a.nhacsi,
                                 theloai = a.theloai
                             }).ToList();

                    if (typeOfSearch == "Tiêu Đề")
                    {

                        foreach (var c in k)
                        {
                            string nameUnsign = convertToUnSign(c.tenbaihat).ToLower();
                            if (nameUnsign.Contains(keyWordUnsign.ToLower()) || c.tenbaihat.ToLower().Contains(keyWord1.ToLower()))
                            {
                                listResult.Add(c);
                            }
                        }


                    }
                    else
                    {

                        foreach (var c in k)
                        {
                            string nameUnsign = convertToUnSign(c.theloai).ToLower();
                            if (nameUnsign.Contains(keyWordUnsign.ToLower()) || c.theloai.ToLower().Contains(keyWord1.ToLower()))
                            {
                                listResult.Add(c);
                            }
                        }

                    }

                    if (listResult.Count == 0)
                    {
                        await this.ShowMessageAsync("Thông Báo", "Không tìm thấy dữ liệu", MessageDialogStyle.Affirmative);

                    }
                }


                gridlistMusic.ItemsSource = listResult;

            }
        }

        //hàm bỏ dấu trong c#
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        private void Switch1(object sender, RoutedEventArgs e)//hát offline
        {

            GetListKaraoke();



        }
        private void Switch2(object sender, RoutedEventArgs e)//hát online
        {
            GetListKaraoke();


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
