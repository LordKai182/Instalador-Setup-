using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Util;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ASArquiteruraData.RepositoryInterfaces;
using ASArquiteruraData.Repository;
using ASArquiteruraData;
using Newtonsoft.Json;
using xDialog;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.Windows.Threading;
using WPFSpark;
using System.IO;
using System.Resources;
using System.Reflection;

namespace InstaladorWebService.Forms
{
    /// <summary>
    /// Interaction logic for usrConfiguracaoAplicacao.xaml
    /// </summary>
    public partial class usrConfiguracaoAplicacao : System.Windows.Controls.UserControl
    {
        string strProcesso = string.Empty;
        public Utilitarios util = new Utilitarios();
        public Itb_terminalRepository TermLocal = new tb_terminalRepository();
        public WebServiceServidor.Service1SoapClient sincronia = new WebServiceServidor.Service1SoapClient();
        public bool nfce = false;
        private System.Timers.Timer bTimer;
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
        int counter = 0;
        bool isBGWorking = false;
        public string resp = string.Empty;
        BackgroundWorker bgWorker;
        public class lista
        {
            string texto { get; set; }
            string img { get; set; }
        }

        bool SrvPdv = false;
 
        public usrConfiguracaoAplicacao(string resposta)
        {
            InitializeComponent();
            resp = resposta;
            this.Loaded += new RoutedEventHandler(UserControl_Loaded_1);
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnWorkCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgress);
            Pbprogresso.Visibility = Visibility.Hidden;

            #region LISTA UNEG
            Items = new List<Item>();
          
            string[] strSplit = resposta.Split('|');
            for (int i = 01; i < strSplit.Count(); i++)
            {
                Item ItemToAdd = new Item();
                ItemToAdd.Name = strSplit[i].ToString();
                ItemToAdd.PictureID = 0;

                Items.Add(ItemToAdd);
                
            }
            
            this.lstItensVerificados.DataContext = this;
#endregion


        }
    
        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Global._InstalaServidor)
            {
                System.Windows.Forms.DialogResult resultSRV = MsgBox.Show("Deseja Instalar o [WbPDV] Neste Servidor ? ", "WB Instalador", MsgBox.Buttons.YesNo, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                if (resultSRV.ToString().Contains("Yes"))
                {

                    SrvPdv = true;

                }
            }
           
            
            if (bgWorker.CancellationPending)
                return;

            StatusMessage msg = new StatusMessage();
            msg.Message = "Inciando Instalação!";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(750);

            #region Desempacotando Itens

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Desempacotando Itens ";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Desempacotando Itens  : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);
            if (!Directory.Exists(@"C:/Temp/Working"))
            {
                Directory.CreateDirectory(@"C:/Temp/Working");
            }
            
            File.WriteAllBytes(@"C:/Temp/Working/WBPDV.zip", Properties.Resources.WBPDV);
            
            File.WriteAllBytes(@"C:/Temp/Working/GWB.zip", Properties.Resources.GWB);

            File.WriteAllBytes(@"C:/Temp/Working/Crystal13.0532BIT.msi", Properties.Resources.Crystal13_0532BIT);
            File.WriteAllBytes(@"C:/Temp/Working/DB.zip", Properties.Resources.DB);

            Thread.Sleep(200);

        

             ZIP.Extract(@"C:\\Temp\\Working\\WBPDV.zip", @"C:\\Temp\\Working");
             
             ZIP.Extract(@"C:/Temp/Working/GWB.zip", @"C:/Temp/Working");
           
            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Desempacotando Itens  : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(750);
            if (!Global._InstalaServidor && Directory.Exists(@"C:\inetpub\wwwroot\Teste"))
            {
                #region INSTALA WEB SERVICE

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Configurando [Web Service]";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(300);

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Configurando [Web Service] : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                File.WriteAllBytes(@"C:/Temp/Working/WebService.zip", Properties.Resources.WebServiceASA);
                ZIP.Extract(@"C:\\Temp\\Working\\WebService.zip", @"C:\inetpub\wwwroot\Teste");
                Thread.Sleep(300);
                util.DesableFirewall();
                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Configurando [Web Service] : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(750);
                #endregion
            }
            #endregion

            #region DESABILITA FIREWALL
          
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Desabilitando Firewall";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Desabilitando Firewall : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);
            util.DesableFirewall();
            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Desabilitando Firewall : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(750);
            #endregion

            #region INSTALABANCO DE DADOS
          
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Instalação [SQL SERVER 2008 R2], Instalando";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);


            if (bgWorker.CancellationPending)
                return;
            if(util.InstallSQLEXPRESS(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Tools", "SQLEXPR_PTB.exe")))
            {
                msg.Message = String.Format("Instalação [SQL SERVER 2008 R2], Concluida com Sucesso..", 0);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
                Thread.Sleep(300);
            }

            Thread.Sleep(750);
            #endregion

            #region INSTALA BANCO DE DADOS
            
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Instalando Banco de dados  [db_WB]";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(500);
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Instalando Banco de dados [db_WB] : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(300);
            if (util.criarBancoSQL(System.IO.Path.Combine(@"C:/Temp/Working", "DB.zip")))
            {
                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Instalando Banco de dados  [db_WB] : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }
            }
            Thread.Sleep(750);
            #endregion

            #region IF SERVIDOR | PDV
           
            if (!Global._InstalaServidor)
            {
               //Global._NumeroTerm = Convert.ToInt32(sincronia.ContaMaquina()) + 1;

                #region ATIVAR REGISTROS

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando Programas!";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(750);
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando Programas : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                #region INSTALACAO
              
                    util.InstalarAplicacao(@"C:\\Temp\\Working\\G-WB.msi");
                    util.InstalarAplicacao(@"C:\\Temp\\Working\\G-WB.msi");
             

               
              
              
                //  util.DirectoryCopy(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "CliSiTef"),@"C:", true);
               

                #endregion

                Thread.Sleep(300);

                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Instalando Programas : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(500);

                #endregion

                #region INSTALADORES CRIANDO TERMINAL

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Criando Terminal!";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(750);
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Criando Terminal : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                #region CADASTRO PDV
                //if (sincronia.ServiceIdentificaTerminal(Global._serialHd) == "[]")
                //{
                    //criaTerm();
               // }
                //else
                //{
                //    Dispatcher.Invoke(
                //    DispatcherPriority.Normal,
                //    (Action)(() => { 
                    
                //    tb_terminal TermHerdado = new tb_terminal();
                //    TermHerdado = JsonConvert.DeserializeObject<List<tb_terminal>>(sincronia.ServiceIdentificaTerminal(Global._serialHd))[0];
                //    TermHerdado.te_nfce = nfce;
                //    TermHerdado.uneg_id = null;
                //    TermLocal.Add(TermHerdado);
                //    util.SetMachineName(TermHerdado.te_nome);
                    
                //    }));
                //}


                #endregion

                Thread.Sleep(300);

                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Criando Terminal : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(500);

                #endregion
              
                }
            if (!Global._InstalaServidor)
            {

                #region ATIVAR REGISTROS
               // util.ExecutaProcesso(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319 \ aspnet_regiis -i");
                Thread.Sleep(100);
               // util.ExecutaProcesso(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319 \ aspnet_regbrowsers -i");
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Ativando Recursos do NET!";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(750);
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Ativando Recursos do NET : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                #region INSTALACAO
                util.SetMachineName("Servidor");

                #endregion

                Thread.Sleep(300);

                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Ativando Recursos do NET : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(500);

                #endregion

                #region COPIANDO ARQUIVOS E INSTALANDO MSI

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Criando WebService!";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(750);
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Criando WebService : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                #region INSTALACAO
                util.InstalarAplicacao(@"C:\\Temp\\Working\\GWB.msi");
                //util.InstalarAplicacao(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "ASAsysUpdateServidorSetup.msi"));
                //if (util.DirectoryCopy(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "WS"), @"C:/inetpub/wwwroot/Teste", true))
                //{
                //    Thread.Sleep(300);

                    for (int i = 1; i < 10; i++)
                    {
                        if (bgWorker.CancellationPending)
                            return;

                        msg.Message = String.Format("Criando WebService : {0}%", (i + 1) * 10);
                        msg.IsAnimated = false;
                        bgWorker.ReportProgress(0, msg);
                        Thread.Sleep(300);
                    }
               // }

                #endregion



                Thread.Sleep(500);

                #endregion

                #region CADASTRAR SERVIDOR

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando [Crystal Reports]";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(750);
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando [Crystal Reports] : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                #region INSTALACAO
                util.InstalarAplicacao(@"C:\\Temp\\Working\\Crystal13.0532BIT.msi");
                #endregion

                Thread.Sleep(300);

                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Instalando [Crystal Reports] : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(500);

                #endregion

                if(SrvPdv)
                {
                    #region INSTALACAO PDV SERVIDOR
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = "Instalando [WB-PDV]. : 10%";
                    msg.IsAnimated = true;
                    bgWorker.ReportProgress(0, msg);

                    util.InstalarAplicacao( @"C:\\Temp\\Working\\WbPDV.msi");
                    //Directory.Delete(@"C:/Temp");

                    for (int i = 1; i < 10; i++)
                    {
                        if (bgWorker.CancellationPending)
                            return;

                        msg.Message = String.Format(" [WB-PDV] Instalado com Sucesso. : {0}%", (i + 1) * 10);
                        msg.IsAnimated = false;
                        bgWorker.ReportProgress(0, msg);



                        Thread.Sleep(300);
                    }


                    #endregion

                    Global._NumeroTerm = 2;
                    //criaTerm();
             
                }
            }
            #endregion

            if (Global._InstalaServidor)
            {
                #region DESABILITA FIREWALL

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando [WBPDV]";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(300);

                if (bgWorker.CancellationPending)
                    return;

                msg.Message = "Instalando [WBPDV] : 10%";
                msg.IsAnimated = true;
                bgWorker.ReportProgress(0, msg);

                Thread.Sleep(300);
                
                for (int i = 1; i < 10; i++)
                {
                    if (bgWorker.CancellationPending)
                        return;

                    msg.Message = String.Format("Instalando [WBPDV] : {0}%", (i + 1) * 10);
                    msg.IsAnimated = false;
                    bgWorker.ReportProgress(0, msg);
                    Thread.Sleep(300);
                }

                Thread.Sleep(750);
                #endregion
            }
            #region

            #region REINICIA

            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Instalação Concluida com sucesso.";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);

            Thread.Sleep(750);
            if (bgWorker.CancellationPending)
                return;

            msg.Message = "Reiniciando Computador : 10%";
            msg.IsAnimated = true;
            bgWorker.ReportProgress(0, msg);
            Directory.Delete(@"C:/Temp/",true);
            if (!Global._InstalaServidor)
            {
                
                Util.MysqlCon utilitarios = new Util.MysqlCon();
                utilitarios.marcaUneg(Global._CNPJ, Global._serialHd);
            }

            Thread.Sleep(300);

            for (int i = 1; i < 10; i++)
            {
                if (bgWorker.CancellationPending)
                    return;

                msg.Message = String.Format("Reiniciando Computador : {0}%", (i + 1) * 10);
                msg.IsAnimated = false;
                bgWorker.ReportProgress(0, msg);
              
               
                
                Thread.Sleep(300);
            }
            //Dispatcher.Invoke(
            //   DispatcherPriority.Normal,
            //   (Action)(() => { Process.Start("shutdown", "/s /t 0"); }));
           

            #endregion

        }

        void OnProgress(object sender, ProgressChangedEventArgs e)
        {
            StatusMessage msg = e.UserState as StatusMessage;
            if (msg != null)
            {
                customStatusBar.SetStatus(msg.Message, msg.IsAnimated);
            }
        }

        void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isBGWorking = false;
            // StartBtn.IsEnabled = true;

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            btnInstala.IsEnabled = false;
            Pbprogresso.Visibility = Visibility.Visible;
            btnInstala.Visibility = Visibility.Hidden;
            bgWorker.RunWorkerAsync();
            isBGWorking = true;

          

        }


        private bool criaTerm()
        {
            try
            {
              
            #region CRIA TERM
            tb_terminal terminal = new tb_terminal()
            {
                te_id_terminal = Global._NumeroTerm,
                uneg_id = null,
                te_status = "A",
                te_servidor_loja = false,
                te_nome_rede = "CAIXA" + Global._NumeroTerm.ToString().PadLeft(3, '0'),
                te_nome = "CAIXA" + Global._NumeroTerm.ToString().PadLeft(3, '0'),
                te_database = "db_loja",
                te_terminal_atualizado = true,
                te_serie_hd = Global._serialHd,
                te_nfce = nfce,

            };
            #endregion
            TermLocal.Add(terminal);

           // sincronia.ServiceInserirTerminal(JsonConvert.SerializeObject(TermLocal.GetAll()));
            util.SetMachineName(terminal.te_nome);
            return true;  


            }
            catch
            {
                return false;
            }
        
        }
        private bool criaServ()
        {
            try
            {

                #region CRIA TERM
                tb_terminal terminal = new tb_terminal()
                {
                    te_id_terminal = 1,
                    uneg_id = null,
                    te_status = "A",
                    te_servidor_loja = true,
                    te_nome_rede = "Servidor",
                    te_nome = "Servidor",
                    te_database = "db_wb",
                    te_terminal_atualizado = true,
                    te_serie_hd = Global._serialHd,
                    te_caminho_servidor = "NT",

                };
                #endregion
                TermLocal.Add(terminal);

                //sincronia.ServiceInserirTerminal(JsonConvert.SerializeObject(TermLocal.GetAll()));
              
                return true;


            }
            catch
            {
                return false;
            }

        }
            #endregion
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            customStatusBar.FadeOutDirection = StatusDirection.Right;
            customStatusBar.FadeOutDistance = 500;
            customStatusBar.FadeOutDuration = new Duration(TimeSpan.FromSeconds(1));
            customStatusBar.MoveDuration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        #region TESTADOS

        public void instalacaoPDV()
        {
           
            Global._NumeroTerm = Convert.ToInt32(sincronia.ContaMaquina()) + 1;
          

            #region INSTALAÇÂO TERMINAL

          
                if (sincronia.ServiceIdentificaTerminal(Global._serialHd) == "[]")
                    {
                        if (util.InstalarAplicacao(@"C:\\Temp\\Working\\WbPDV.msi"))
                        {
                            criaTerm();
                        }


                    }
                    else
                    {
                        System.Windows.Forms.DialogResult result = MsgBox.Show("Existe um terminal ja cadastrado para esta maquina as configurações serão Herdadas. ", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
                        tb_terminal TermHerdado = new tb_terminal();
                        TermHerdado = JsonConvert.DeserializeObject<List<tb_terminal>>(sincronia.ServiceIdentificaTerminal(Global._serialHd))[0];
                        TermHerdado.te_nfce = nfce;
                        TermLocal.Add(TermHerdado);
                        util.SetMachineName(TermHerdado.te_nome);



                    }
                
               
            
           



        }

        //        #endregion
        //        System.Windows.Forms.DialogResult resultFim = MsgBox.Show("Instalação concluida com sucesso.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
        //        Dispatcher.Invoke(
        //        DispatcherPriority.Normal,
        //        (Action)(() => { Process.Start("shutdown", "/s /t 0"); })
        //           );
        //}
        //    private void ProgressoMsg(int valor, string Mensagem)
        //    {   

        //        txtNota.Text = Mensagem;


        //    }
        //  public void AtivarTimer()
        //{
        //    bTimer = new System.Timers.Timer();
        //    bTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //    bTimer.Interval = 1500;
        //    bTimer.Enabled = true;
        //    bTimer.Start();
        //}
        //    private void OnTimedEvent(object sender, ElapsedEventArgs e)
        //    {
        //        if (Global._InstalaServidor)
        //        {



        //            instalacaoPDV();

        //        }
        //        if (!Global._InstalaServidor)
        //        {


        //            instalacaoServidor();

        //        }
        //    }
        //    private void Button_Click_1(object sender, RoutedEventArgs e)
        //    {
        //      //  Items.Clear();

        //    }
        //    private void Button_Click_2(object sender, RoutedEventArgs e)
        //    {
        //        AtivarTimer();
        //        txtNota.Text = "Instalando Por Favor Aguarde..";
        //        Pbprogresso.Visibility = Visibility.Visible;

        //        btnInstala.Visibility = Visibility.Hidden;

        //    }
        //    public void instalacaoServidor()
        //    {
        //        bTimer.Enabled = false;
        //        util.DesableFirewall();
        //        if (util.InstallSQLEXPRESS(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "SQLEXPR_PTB.exe")))
        //        {
        //            util.SetMachineName("Servidor");
        //            util.ExecutaProcesso(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319 \aspnet_regiis -i");
        //            util.ExecutaProcesso(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319 \aspnet_regbrowsers -i");
        //        }
        //        else
        //        {
        //            System.Windows.Forms.DialogResult result = MsgBox.Show("Houve um erro ao Instalar o banco de dados [SQLSERVER 2008 R2]", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
        //            Dispatcher.Invoke(
        //            DispatcherPriority.Normal,
        //            (Action)(() => { System.Windows.Application.Current.Shutdown(); }));

        //        }
        //        if (util.criarBancoSQL(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "BD.zip")))
        //        {

        //        }else
        //        {

        //            System.Windows.Forms.DialogResult result = MsgBox.Show("Houve um erro ao Criar o banco de dados [SQLSERVER 2008 R2]", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);

        //            Dispatcher.Invoke(
        //            DispatcherPriority.Normal,
        //            (Action)(() => { System.Windows.Application.Current.Shutdown(); }));

        //        }
        //          if (util.DirectoryCopy(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "WS"), @"C:/inetpub/wwwroot/Teste", true))
        //          {
        //             util.InstalarAplicacao(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "ASAsysServidor.Setup.msi"));

        //             util.InstalarAplicacao(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Teste", "ASAsysServidor.Setup.msi"));
        //             criaServ();

        //          }
        //          else
        //          {

        //          }


        //        System.Windows.Forms.DialogResult resultFim = MsgBox.Show("Instalação concluida com sucesso.", "WB Instalador", MsgBox.Buttons.OK, MsgBox.Icon.Warning, MsgBox.AnimateStyle.FadeIn);
        //        Dispatcher.Invoke(
        //        DispatcherPriority.Normal,
        //        (Action)(() => { Process.Start("shutdown", "/s /t 0"); }));

        //    }
       
        #endregion


        #endregion

    }
}
           