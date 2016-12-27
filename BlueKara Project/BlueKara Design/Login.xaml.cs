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
using BlueKara_Design.Database;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using System.Windows.Threading;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        DeepKaraEntities de = new DeepKaraEntities();
        string _PlayerID;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void GoIntoMain(string _PlayerID)
        {
            MainHome a = new MainHome(_PlayerID);
            a.Show();
            this.Close();
        }

        private bool CheckLogin()
        {
            var k = de.PLAYERs.Select(a => new { a.Username, a.Password, a.PlayerID }).ToList();

            foreach (var a in k)
            {
                if (a.Username == tblogin.Text && a.Password == tbpass.Password)
                {

                    _PlayerID = a.PlayerID;
                    return true;
                }
            }
            return false;

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

        private async void btnLogin(object sender, MouseButtonEventArgs e)
        {
           
            bool result=false;
            //code asyn and await complete, avoid using another thread notification error
            await Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            {
                
                result =CheckLogin();
                ChangeProgressRing();

            },null);

            
           
            if (result == true)
            {

                GoIntoMain(_PlayerID);
                ChangeProgressRingagain();

            }
            else
            {
                await this.ShowMessageAsync("Thông Báo", "Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại", MessageDialogStyle.Affirmative);
                ChangeProgressRingagain();

            }
        }
        private void ChangeProgressRingagain()
        {
            Progressring.Visibility = Visibility.Hidden;
            GifBackground.Visibility = Visibility.Hidden;
            maingrLogin.IsEnabled = true;
        }

        private void ChangeProgressRing()
        {
            Progressring.Visibility = Visibility.Visible;
            GifBackground.Visibility = Visibility.Visible;
            maingrLogin.IsEnabled = false;

        }
    }
}
