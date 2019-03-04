// Para achar o Configuration é necessário adicionar uma referencia em references entrando em Assembly e colocando na mão o System.Configuration
using System.Configuration;
using System.IO;

namespace Eventos.IO.TestesAutomatizados.Config
{
    public class ConfigurationHelper
    {
        // Instruções para Melhor Entendimento
        // {0}{1}{2} Indica por exemplo que estamos esperando 3 parâmetros após a vírgula
        // GetDirectoryName irá bustar o nome do diretório da pasta ScreenShots
        // Estas chaves foram configuradas em App.Config, mas poderiam estar em qualquer outro lugar
        public static string SiteUrl        => ConfigurationManager.AppSettings["SiteUrl"];
        public static string Site           => ConfigurationManager.AppSettings["Site"];
        public static string RegisterUrl    => string.Format("{0}{1}",SiteUrl,ConfigurationManager.AppSettings["RegisterUrl"]);
        public static string LoginUrl       => string.Format("{0}{1}",SiteUrl, ConfigurationManager.AppSettings["LoginUrl"]);
        public static string ChromeDrive    => string.Format("{0}", ConfigurationManager.AppSettings["ChromeDrive"]);
        public static string FolderPath     => Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        public static string FolderPicture  => string.Format("{0}{1}", FolderPath, ConfigurationManager.AppSettings["FolderPicture"]);
        public static string TestUserName   => ConfigurationManager.AppSettings["TestUserName"];
        public static string TestPassword   => ConfigurationManager.AppSettings["TestPassword"];

    }
}
