using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using SynchronicWorldService;

namespace SynchronicWorldService.Installer
{
    public partial class Service1 : ServiceBase
    {
        internal static ServiceHost SwServiceHost = null; 

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (SwServiceHost != null)
            {
                SwServiceHost.Close();
            }
            SwServiceHost = new ServiceHost(typeof(Service1));
            SwServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (SwServiceHost != null)
            {
                SwServiceHost.Close();
                SwServiceHost = null;
            }
        }
    }
}
