using InstaladorWebService.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Util;
namespace InstaladorWebService
{
    public class Global
    {
       public static  Utilitarios util = new Utilitarios();
       public static int _NumeroTerm ;
       public static string _serialHd = util.GetHDDSerialNumber("C");
       public static string _memoriaFisica = util.MemoriaFisica();
       public static string _sistemaOperacional = util.versaoWindows();
       public static string _sqlServer = util.GetSQLStatus();
       public static string _dotNet = util.InstalledDotNetVersions().Max(s => s.Major).ToString();
       public static bool _arquivosNes = util.VerificarArquivosInstall(System.AppDomain.CurrentDomain.BaseDirectory);
       public static bool _InstalaServidor = util.VerifcaNaRede("Servidor");
       public static string _PastaASA = string.Empty;
       public static string _CNPJ = string.Empty;
       public class Formularios
       {
           public static usrSplashFomr1 _usrForm1;
           public static usrConfiguracaoAplicacao _Configinstalacao;
           public static Splash _Splash;
       
       }
    }
}
