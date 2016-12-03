using System;
using System.Diagnostics;
using System.Windows.Forms;
using WHLocator;

namespace WindowsFormsApplication3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var listener = new AuthorizeListener();
            //listener.Run();

            log4net.Config.XmlConfigurator.Configure();

            Application.Run(new WindowMonitoring());

            Application.Exit(); 

            
  




            //listener.Stop();
        }
    }
}
