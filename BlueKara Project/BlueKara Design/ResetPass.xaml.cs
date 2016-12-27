using BlueKara_Design.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for ResetPass.xaml
    /// </summary>
    public partial class ResetPass
    {
        DeepKaraEntities de = new DeepKaraEntities();
        public ResetPass()
        {
            InitializeComponent();

        }

        private void Gobacklogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();

        }

        private async void genarateNewPass(object sender, MouseButtonEventArgs e)
        {
            List<PlayerLoss> k = new List<PlayerLoss>();

            ChangeProgressRing();
            await Task.Run(() =>
             {
                 k = de.PLAYERs.Select(a => new PlayerLoss { answer= a.Answer,phone= a.PhoneNumber,email= a.Email,playerid= a.PlayerID }).ToList();

             });

            ChangeProgressRingagain();


            foreach (PlayerLoss c in k)
            {
                if (c.answer.ToLower() == txbAnswer.Text.ToLower() && c.phone == txbPhone.Text && c.email.ToLower() == txbEmail.Text.ToLower())
                {
                    NewPass.Text = RandomPass();
                    var selectChange = (from a in de.PLAYERs
                                        where c.playerid == a.PlayerID
                                        select a).FirstOrDefault();

                    if (selectChange != null)
                    {
                        selectChange.Password = NewPass.Text;
                        de.SaveChanges();
                    }
                    return;
                }
            }

            await this.ShowMessageAsync("Thông báo", "Thông tin không chính xác. Vui lòng thử lại.");
            return;
        }

        private string RandomPass()
        {
            return Generate(8, 10);
        }

        private string Generate(int minLength, int maxLength)
        {
            string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
            string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
            string PASSWORD_CHARS_NUMERIC = "23456789";
            string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;


            char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

            int[] charsLeftInGroup = new int[charGroups.Length];

            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;


            int[] leftGroupsOrder = new int[charGroups.Length];


            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;


            byte[] randomBytes = new byte[4];


            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);


            int seed = BitConverter.ToInt32(randomBytes, 0);


            Random random = new Random(seed);


            char[] password = null;


            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];


            int nextCharIdx;


            int nextGroupIdx;

            int nextLeftGroupsOrderIdx;


            int lastCharIdx;


            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;


            for (int i = 0; i < password.Length; i++)
            {

                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);


                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];


                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;


                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                password[i] = charGroups[nextGroupIdx][nextCharIdx];


                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;

                else
                {

                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }

                    charsLeftInGroup[nextGroupIdx]--;
                }


                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

                else
                {

                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }

                    lastLeftGroupsOrderIdx--;
                }
            }

            return new string(password);

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
    }

    internal class PlayerLoss
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string answer { get; set; }

        public string playerid { get; set; }

    }
}
