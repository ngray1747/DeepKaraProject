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

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for MainHome.xaml
    /// </summary>
    public partial class MainHome
    {
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

        private void OpenFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutInfo.IsOpen = true;
            closefyout.Visibility = Visibility.Visible;
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
