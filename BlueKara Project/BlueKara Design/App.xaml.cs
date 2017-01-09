using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BlueKara_Design
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int a = 6000;

        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();
            Stopwatch timer = new Stopwatch();
            timer.Start();

            base.OnStartup(e);
            MainWindow main = new MainWindow();

            timer.Stop();
            int remaintime = a - (int)timer.ElapsedMilliseconds;
            if(remaintime>0)
            {
                Thread.Sleep(remaintime);
            }

            splash.Close();


        }
        protected override void OnExit(ExitEventArgs e)
        {

            base.OnExit(e);


       

        }
    }
}
