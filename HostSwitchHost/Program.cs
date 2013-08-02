using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HostSwitchLib;

namespace HostSwitchHost
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
            var hosts = new HostSwitcher();
            var icon = new SwitcherNotificationIcon(hosts);
            Application.Run();
        }
    }
}
