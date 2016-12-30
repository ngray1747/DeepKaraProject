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
using BlueKara_Design.Database;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount
    {
        string filename;
        string url;
        DeepKaraEntities de = new DeepKaraEntities();

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
        private int UploadToFTP()
        {
            //get extension
            string[] _fileNameExt = filename.Split(' ');
            string fileaNameExt = _fileNameExt[_fileNameExt.Count() - 1];

            var k = de.AVATARs.Select(a => a.AvatarID);
            filename = string.Format("Avatar{0}.{1}", k.Count() + 1, fileaNameExt);

            string server = "ftp://123.31.34.57";
            string username = "Administrator";
            string password = "Tietkhaiky2";
            FtpWebRequest upload = (FtpWebRequest)WebRequest.Create(new Uri(String.Format("{0}/Image/{1}", server, filename)));
            upload.Method = WebRequestMethods.Ftp.UploadFile;
            upload.Credentials = new NetworkCredential(username, password);
            Stream str = upload.GetRequestStream();
            FileStream fs = File.OpenRead(url);
            byte[] buffer = new byte[1024];
            double total = (double)fs.Length;
            int byteread = 0;
            double read = 0;

            do
            {
                byteread = fs.Read(buffer, 0, 1024);
                str.Write(buffer, 0, byteread);
                read += (double)byteread;


            } while (byteread != 0);
            fs.Close();
            str.Close();
            return 0;
        }
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = ".";
            file.Filter = "Image File (*.jpeg,*.png,*.jpg) | *.jpeg;*.png;*.jpg";
            Nullable<bool> a = file.ShowDialog();
            if (a == true)
            {
                string[] splitUrl = file.FileName.Split('\\');
                filename = splitUrl[splitUrl.Count() - 1];
                url = file.FileName;



                Task<int> _task1 = new Task<int>(UploadToFTP);

                _task1.Start();
                ChangeProgressRing();
                int c = await _task1;

                Task<int> _task2 = new Task<int>(DowloadFromFTP);
                _task2.Start();
                ChangeProgressRing();
                int d = await _task2;

                ChangeProgressRingagain();


                Image.Source = new BitmapImage(new Uri(String.Format("C:\\ShareFolderMusic\\Image\\{0}", filename)));


            }
        }

        private int DowloadFromFTP()
        {

            string inputfilepath = String.Format("C:\\ShareFolderMusic\\Image\\{0}", filename);

            string ftphost = "123.31.34.57";
            string ftpfilepath = String.Format("/Image/{0}", filename);

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

        private void ChangeProgressRingagain()
        {
            Progressring.Visibility = Visibility.Hidden;
            GifBackground.Visibility = Visibility.Hidden;
            MainGrid.IsEnabled = true;
        }

        private void ChangeProgressRing()
        {
            Progressring.Visibility = Visibility.Visible;
            GifBackground.Visibility = Visibility.Visible;
            MainGrid.IsEnabled = false;

        }



        private async void SubmitProfile(object sender, MouseButtonEventArgs e)
        {



            PLAYER pl = new PLAYER();
            LEVEL level = new LEVEL();
            AVATAR av = new AVATAR();
            try
            {
                //create ID Auto
                var k = de.PLAYERs.Select(a => a.PlayerID);
                string id = String.Format("PL{0}", k.Count() + 1);
                pl.PlayerID = id;

                pl.Email = Email.Text;
                pl.Username = Username.Text;
                pl.Password = Password.Password;
                pl.Job = Job.Text;
                pl.BirthDate = DateBirth.SelectedDate.Value;

                pl.FullName = FullName.Text;
                pl.PhoneNumber = Sdt.Text;
                pl.FacebookURL = FacebookLink.Text;
                pl.SecurityQuestion = ((ComboBoxItem)Question.SelectedItem).Content.ToString();
                pl.Answer = Answer.Text;
                pl.Scores = 0;


                var k1 = de.AVATARs.Select(a1 => a1.AvatarID);
                av.AvatarID = String.Format("AV{0}", k1.Count() + 1);
                av.URL = filename;
                av.PlayerID = id;

                level.PlayerID = id;
                level.Name = "Level 1";
                level.MaxScore = 1000;
                level.RemainScoreToUp = 1000;

                var k2 = de.LEVELs.Select(a1 => a1.LevelID);
                level.LevelID = string.Format("LV{0}", k2.Count() + 1);

            }
            catch { }

            //you need to choose avatar

            //kiểm tra sự tồn tại email,sdt,ten dang nhap
            var email = de.PLAYERs.Select(a => a.Email);
            var sdt = de.PLAYERs.Select(a => a.PhoneNumber);
            var usename = de.PLAYERs.Select(a => a.Username);

            foreach (string a in email)
            {
                if (Email.Text.ToLower() == a)
                {
                    await this.ShowMessageAsync("Thông Báo", "Email này đã được đăng ký trước đó.", MessageDialogStyle.Affirmative);
                    return;
                }

            }
            foreach (string a in sdt)
            {
                if (Sdt.Text.ToLower() == a)
                {
                    await this.ShowMessageAsync("Thông Báo", "Số điện thoại này đã được sử dụng", MessageDialogStyle.Affirmative);
                    return;
                }
            }
            foreach (string a in usename)
            {
                if (Username.Text.ToLower() == a)
                {
                    await this.ShowMessageAsync("Thông Báo", "Tên đăng nhập này đã tồn tại", MessageDialogStyle.Affirmative);
                    return;
                }
            }


            if (filename == null || FacebookLink.Text == null || Answer.Text == null || Email.Text == null || Sdt.Text == null)
            {
                await this.ShowMessageAsync("Thông Báo", "Bạn cần phải điền đầy đủ tất cả thông tin trước khi hoàn tất đăng ký.", MessageDialogStyle.Affirmative);

            }
            else
            {
                ChangeProgressRing();
                await Task.Run(() =>
                {
                    de.PLAYERs.Add(pl);
                    de.AVATARs.Add(av);
                    de.LEVELs.Add(level);
                    de.SaveChanges();


                });

                ChangeProgressRingagain();

                await this.ShowMessageAsync("Thông Báo", "Chúc mừng bạn đã đăng ký thành công. Mời bạn đăng nhập để sử dụng chương trình.", MessageDialogStyle.Affirmative);
                Goback(null, null);

            }

        }

        private void _PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
