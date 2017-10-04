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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xDialog;
using InstaladorWebService.Properties;
using System.IO;
using System.Timers;
using System.Threading;
using System.Windows.Threading;
namespace InstaladorWebService.Forms
{
    /// <summary>
    /// Interaction logic for usrSplashFomr1.xaml
    /// </summary>
    public partial class usrSplashFomr1 : UserControl
    {
        private System.Timers.Timer aTimer;
        public class Item
        {
            public int PictureID { get; set; }
            public string Name { get; set; }
            public string PictureString
            {
                get { return "/Images/" + PictureID.ToString() + ".png"; }
            }
        }
        public List<Item> Items { get; set; }
        public List<Item> itemLaco = new List<Item>();
        public Util.Utilitarios util = new Util.Utilitarios();
        int bypass = 0;
        bool Sql = false;
        private enum OS
        {
            Windows_7,
            Windows_8,
            Windows_10,
         }
        public class lista
        {
            string texto { get; set; }
            string img { get; set; }
        }

        Splash Form;
        public usrSplashFomr1(Splash form)
        {

            InitializeComponent();

            Form = form;
            sprocketControl1.Visibility = Visibility.Hidden;
            Inicia();
          
        }
        public void AtivarTimer()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1500;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Desistalacao();
        }
        public void Inicia()
        {
            AdcionaList("Serial do HD :  " + Global._serialHd, true);
            AdcionaList("Versão DotNet : " + Global._dotNet, true);
            AdcionaList("Versão da CLR : " + System.Environment.Version, true);

            #region VERIFICA MEMORIA
            if (Convert.ToInt32(Global._memoriaFisica) > 4)
            {
                AdcionaList("Memoria Fisica : Memoria em GB : " + Global._memoriaFisica, true);
            }
            else
            {
                AdcionaList("Memoria Fisica : [ERRO] Memoria menor que 4GB", false);
            }
            #endregion

            #region VERIFICA OS
            try
            {
                #region SWITCH
                switch ((OS)Enum.Parse(typeof(OS), Global._sistemaOperacional))
                {
                    case OS.Windows_10:

                        AdcionaList("Sistema Operacional : " + Global._sistemaOperacional.Replace("_", " "), true);

                        break;
                    case OS.Windows_7:

                        AdcionaList("Sistema Operacional : " + Global._sistemaOperacional.Replace("_", " "), true);

                        break;
                    case OS.Windows_8:

                        AdcionaList("Sistema Operacional : " + Global._sistemaOperacional.Replace("_", " "), true);

                        break;



                }
                #endregion
            }
            catch
            {

                AdcionaList("Sistema Operacional : [ERRO] OS Não Suportada.", false);
            }
            #endregion

            #region VERIFCA ARQUIVOS
            if (Directory.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"Tools")))
            {
                AdcionaList("Arquivos Para Instalação : Verificados.", true);
            }
            if (!Directory.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Tools")))
            {
                AdcionaList("Arquivos Para Instalação : [ERRO] Faltando.", false);
                bypass = 1;
            }
            #endregion

            #region VERIFICA TIPO DE INTALAÇÂO
            if (Global._InstalaServidor)
            {

                AdcionaList("Tipo de instalação : [PDV]", true);
            }
            if (!Global._InstalaServidor)
            {
                 AdcionaList("Tipo de instalação : [Servidor]", true);
                if (!Directory.Exists(@"C:\inetpub\wwwroot\Teste"))
                {
                 AdcionaList("WebService: [C:intePub/www/Teste] ERRO.", false);
                }
                if (Directory.Exists(@"C:\inetpub\wwwroot\Teste"))
                {

                    AdcionaList("WebService: [C:/intePub/www/Teste] OK.", true);
                }
            }
            #endregion

            #region VERIFICA PRESENÇA DO SQL SERVER
            if (Global._sqlServer.Equals("Instalado"))
            {
                AdcionaList("Banco de Dados : [ERRO] SQL Instalado.", false);
                Sql = true;


            }
            else
            {

                AdcionaList("Banco de Dados : " + Global._sqlServer, true);

            }
            #endregion

            #region REALOCA LISTAS
            Items = new List<Item>();
            foreach (var item in itemLaco)
            {
                Item ItemToAdd = new Item();
                ItemToAdd.Name = item.Name;
                ItemToAdd.PictureID = item.PictureID;

                Items.Add(ItemToAdd);
            }
            this.lstItensVerificados.DataContext = this;
           
            #endregion


            if (bypass != 0)
            {
              
                System.Windows.Forms.DialogResult result2 = MsgBox.Show("Houve exceçoes na verificação de Requisitos, o WB não proseeguira. Veja o resumo para Resolver os erros.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                

            }
            if(Global._sqlServer.Equals("Instalado"))
            {
                AdcionaList("Banco de Dados : [ERRO] SQL Instalado.", false);
                Sql = true;
                System.Windows.Forms.DialogResult result = MsgBox.Show("Um Banco de dados SQL Server foi encontrado e precisa dser desistalado.", "WB Instalador", MsgBox.Buttons.YesNo, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                if (result.ToString().Contains("Yes"))
                {
                    sprocketControl1.Visibility = Visibility.Visible;
                    lstItensVerificados.Visibility = Visibility.Hidden;
                    btnProsseguir.Visibility = Visibility.Hidden;
                   // lbNota2.Content = "Desinstalando Aguarde.";
                    Thread.Sleep(500);
                    AtivarTimer();

                }
                if (result.ToString().Contains("No"))
                {
                    MsgBox.Show("A instalação Não podera Prosseguir.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                    //Application.Current.Shutdown();
                }

            }
        }
        public void Desistalacao()
        {
            aTimer.Enabled = false;
            util.UnistallSQLEXPRESS(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "SQLEXPR_PTB.exe"));
           
            MsgBox.Show("Desistalações Concluidas Reinicie o Instalador para Nova Verificação.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
            Dispatcher.Invoke(
            DispatcherPriority.Normal,
            (Action)(() => { Application.Current.Shutdown(); })
            );
           
        
        }

        public void AdcionaList(string item, bool marca)
        {
          
            if(marca)
            {
                Item ItemToAdd = new Item();
                ItemToAdd.Name = item;
                ItemToAdd.PictureID = 0;

                itemLaco.Add(ItemToAdd);

            }
            if (!marca)
            {
                Item ItemToAdd = new Item();
                ItemToAdd.Name = item;
                ItemToAdd.PictureID = 1;

                itemLaco.Add(ItemToAdd);
            }
           
        }

        private void btnProsseguir_Click(object sender, RoutedEventArgs e)
        {

            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Digite seu CNPJ para Verificação de permissões", "WB Gestão", "Digite seu CNPJ sem pontos");
            Util.MysqlCon utilitarios = new Util.MysqlCon();
            string tre = utilitarios.SelecionaUneg(UserAnswer);
            if (tre == string.Empty)
            {
                System.Windows.Forms.DialogResult result = MsgBox.Show("CNPJ informado não consta na Base de dados.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
            }
            if (tre == string.Empty)
            {

            }
            else
            {
                string[] strSplit = tre.Split('|');
                if (strSplit.Count() > 1)
                {

                    if (strSplit[5].Length == 23)
                    {
                        strSplit[5] = "Serial HDD Habilitado: " + Global._serialHd;
                    }
                    if (strSplit[5].Length > 23)
                    {
                        if (Global._serialHd != strSplit[5].Substring(23, strSplit[5].Length - 23) && Global._InstalaServidor == false)
                        {
                            MsgBox.Show("Este Computador Não esta autorizado a fazer uma instalaçao de Servidor.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                            
                        }
                    }
                    if (Global._serialHd == strSplit[5].Substring(23, strSplit[5].Length - 23) && strSplit[2].ToString().Substring(10,1) == "N")
                    {
                        Global._CNPJ = UserAnswer;
                        Form.Visibility = Visibility.Hidden;
                        MainWindow FormPrincipal = new MainWindow(tre);
                        FormPrincipal.Show();
                    }
                    //if (Global._InstalaServidor == true)
                    //{
                    //    Global._CNPJ = UserAnswer;
                    //    Form.Visibility = Visibility.Hidden;
                    //    MainWindow FormPrincipal = new MainWindow(tre);
                    //    FormPrincipal.Show();
                    //} 
                    if (Global._serialHd == strSplit[5].Substring(23, strSplit[5].Length - 23) && strSplit[2].ToString().Substring(10, 1) == "S")
                    {
                        MsgBox.Show("Este CNPJ não esta autorizado a fazer instalação. Entre em contato com um administrador.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                    }
                }
                if(strSplit.Count() == 1)
                {
                    MsgBox.Show(strSplit[0].ToString(), "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                }
            }
        }
    }
}
