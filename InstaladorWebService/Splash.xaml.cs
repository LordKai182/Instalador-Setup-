using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Util;
using xDialog;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Timers;
using InstaladorWebService.Forms;
namespace InstaladorWebService
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public   Utilitarios utilidade = new Utilitarios();
      
        public Splash()
        {
            InitializeComponent();
         

            
        }
       
        private void Window_ContentRendered_1(object sender, EventArgs e)
        {

            
            Global.Formularios._usrForm1 = new usrSplashFomr1(this);
            frame.Navigate(Global.Formularios._usrForm1);
        }

        private void frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(10, BordaSplash.ActualHeight, new Duration(TimeSpan.FromMilliseconds(900)));
            frame.BeginAnimation(FrameworkElement.HeightProperty, doubleAnimation);
        }
    }
}
