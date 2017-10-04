
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

using xDialog;
using InstaladorWebService.Forms;

namespace InstaladorWebService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow(string resposta)
        {   
            InitializeComponent();
            resposta = resposta + "|Serial Local HDD: " + Global._serialHd;
            this.Title = "Verificação Unidade de Negócio.";
            Global.Formularios._Configinstalacao = new usrConfiguracaoAplicacao(resposta);
            frame.Navigate(Global.Formularios._Configinstalacao);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // DialogResult result = MsgBox.Show("Are you sure you want to exit?", "Exit", MsgBox.Buttons.YesNo, MsgBox.Icon.Info, MsgBox.AnimateStyle.FadeIn);
            //if (result.ToString().Equals("Yes"))
            //{
            //    System.Windows.MessageBox.Show("Exiting now");
            //}

          
        }

      

        private void frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {


            DoubleAnimation doubleAnimation = new DoubleAnimation(10, 700, new Duration(TimeSpan.FromMilliseconds(700)));
            frame.BeginAnimation(FrameworkElement.WidthProperty, doubleAnimation);
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

      
    }
}
