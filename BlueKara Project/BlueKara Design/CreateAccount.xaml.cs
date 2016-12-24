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

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount
    {
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = ".";
            file.Filter = "Image File (*.jpeg,*.png,*.jpg) | *.jpeg;*.png;*.jpg";
            Nullable<bool> a = file.ShowDialog();
            if(a==true)
            {
                string url = file.FileName;
                Image.Source = new BitmapImage(new Uri(url));



            }
        }

        private void SubmitProfile(object sender, MouseButtonEventArgs e)
        {
            PLAYER pl = new PLAYER();
            //create ID Auto
            var k = de.PLAYERs.Select(a => a.PlayerID);
            pl.PlayerID = String.Format("PL {0}", k.Count() + 1);

            pl.Email = Email.Text;
            pl.Username = Username.Text;
            pl.Password = Password.ToString();
            pl.Job = Job.Text;
            pl.BirthDate = DateBirth.SelectedDate.Value;
            pl.FullName = FullName.Text;
            pl.PhoneNumber = Sdt.Text;
            pl.FacebookURL = FacebookLink.Text;
            pl.SecurityQuestion = ((ComboBoxItem)Question.SelectedItem).Content.ToString();
            pl.Answer = Answer.Text;

            AVATAR av = new AVATAR();
            


        }

      

       
    }
}
