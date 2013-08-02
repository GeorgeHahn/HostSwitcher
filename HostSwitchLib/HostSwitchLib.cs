using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace HostSwitchLib
{
    public class HostSwitcher
    {
        public HostSwitcher()
        {
            Hosts = new List<Host>(HostInterface.GetHosts());
        }

        public List<Host> Hosts { get; private set; }

        public void Save()
        {
            HostInterface.WriteHosts(Hosts);
        }
    }

    public static class HostInterface
    {
        public const string SystemHostsFile = @"C:\Windows\System32\drivers\etc\hosts";

        public static IEnumerable<Host> GetHosts()
        {
            return GetHosts(SystemHostsFile);
        }

        public static IEnumerable<Host> GetHosts(string HostsFile)
        {
            string[] hostsFile = File.ReadAllLines(HostsFile);

            {
                string host_id = null;
                foreach (var line in hostsFile)
                {
                    if (host_id == null) // First line of a 2 line host combo
                    {
                        if (line.StartsWith("##")) // This is a doco line for a host we've inserted
                        {
                            host_id = line.TrimStart(new[] {'#', ' ', '\t'});
                            continue;
                        }
                    }
                    else
                    {
                        bool enabled = !line.Trim().StartsWith("#");
                        var lineparts = line.Split(new[] {" ", "#"}, StringSplitOptions.RemoveEmptyEntries);
                        string host_address = lineparts[0].Replace("#", "");
                        string host_name = lineparts[1];
                        var host = new Host(enabled, host_id, host_name, host_address);
                        yield return host;

                        host_id = null;
                    }
                }
            }
        }

        public static void WriteHosts(IEnumerable<Host> hosts)
        {
            WriteHosts(SystemHostsFile, hosts);
        }

        public static void WriteHosts(string HostsFile, IEnumerable<Host> hosts)
        {
            StringBuilder file = new StringBuilder();
            foreach (var host in hosts)
            {
                string line1 = "## " + host.Identifier;
                string line2 = (host.Enabled ? "" : "#") + host.Address + " " + host.Hostname;

                file.AppendLine(line1);
                file.AppendLine(line2);
                file.AppendLine();
            }

            File.WriteAllText(HostsFile, file.ToString());
        }
    }

    public class Host
    {
        public Host(bool enabled, string identifier, string hostname, string address)
        {
            Enabled = enabled;
            Identifier = identifier;
            Hostname = hostname;
            Address = address;
        }

        public bool Enabled { get; set; }

        public string Identifier { get; set; }

        public string Hostname { get; set; }

        public string Address { get; set; }
    }
}
