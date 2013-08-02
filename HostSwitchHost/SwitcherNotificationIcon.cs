using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HostSwitchLib;

namespace HostSwitchHost
{
    public class SwitcherNotificationIcon
    {
        public SwitcherNotificationIcon(HostSwitcher hosts)
        {
            _contextMenu = new ContextMenuStrip
                {
                    Name = "contextMenu",
                    Size = new System.Drawing.Size(109, 26)
                };

            var icon = new NotifyIcon
                {
                    ContextMenuStrip = this._contextMenu,
                    Icon = new Icon("Icon.ico"),
                    Text = "Host Switcher",
                    Visible = true
                };

            AddHosts(hosts);

            _contextMenu.Items.Add(new ToolStripSeparator());

            var exit = new ToolStripMenuItem("Exit");
            exit.Click += (s, a) =>
                {
                    icon.Visible = false;
                    Application.Exit();
                };
            _contextMenu.Items.Add(exit);
        }

        void AddHost(HostSwitcher switcher, Host host)
        {
            var menuItem = new ToolStripMenuItem
                {
                    Text = (string.IsNullOrWhiteSpace(host.Identifier) ? host.Hostname : host.Identifier),
                    Checked = host.Enabled
                };

            menuItem.Click += (s, a) =>
                {
                    host.Enabled = !menuItem.Checked;
                    menuItem.Checked = host.Enabled;
                    switcher.Save();
                };

            _contextMenu.Items.Add(menuItem);
        }

        private readonly ContextMenuStrip _contextMenu;

        void AddHosts(HostSwitcher hosts)
        {
            foreach (var host in hosts.Hosts)
                AddHost(hosts, host);
        }
    }
}
