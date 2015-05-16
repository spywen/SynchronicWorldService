using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SynchronicWorldService.Installer
{
    /// <summary>
    /// Installer for the TCP WCF application
    /// How to install it :
    /// 1) Compile this project as Release
    /// 2) Copy paste the content of {this project}/bin/Release inside a folder like C:/Services/SynchronicWorldService
    /// 3) Open the folder SynchronicWorldService through a command line console
    /// 4) Execute the command : 'installutil SynchronicWorldService.Installer.exe'.
    /// 5) Open the 'services.msc' app. You will be able to see a new service : 'SynchronicWorldService'. Run it.
    /// 6) If you trying to test it some issues could happened cause by configs problems. Please refer to : https://msdn.microsoft.com/en-us/library/ms731053(v=vs.110).aspx
    /// 7) To uninstall the service, execute the command : 'installutil /u SynchronicWorldService.Installer.exe'
    /// Source : https://msdn.microsoft.com/en-us/library/ff649818.aspx
    /// Remark : not able to install it... cause by tcp issues...
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
