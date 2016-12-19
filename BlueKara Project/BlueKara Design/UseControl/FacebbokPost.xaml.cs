using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BlueKara_Design.UseControl
{
    /// <summary>
    /// Interaction logic for FacebbokPost.xaml
    /// </summary>
    public partial class FacebbokPost : UserControl
    {
        public ObservableCollection<PicClass> Images { get; set; }
        public FacebbokPost()
        {
            InitializeComponent();
            Images = new ObservableCollection<PicClass>();
           
            Images.Add(
                new PicClass() { Image = new BitmapImage(new Uri("../Image/public.png",UriKind.Relative)), Content = "Public" });
            Images.Add(new PicClass() { Image = new BitmapImage(new Uri("../Image/friend.png", UriKind.Relative)), Content = "Friends" });
            Images.Add(new PicClass()
            {
                Image = new BitmapImage(new Uri("../Image/onlyme.png", UriKind.Relative)),
                Content = "Only Me"
            });
            comboBox.ItemsSource = Images;

        }
    }
    public class PicClass
    {
        public BitmapImage Image { get; set; }
        public string Content { get; set; }
    }
}
