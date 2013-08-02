using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HostSwitchLib;
using Xunit;

namespace HostSwitchTests
{
    public class HostInterfaceTests
    {
        [Fact]
        public void TestParsing()
        {
            var hosts = HostInterface.GetHosts("TestHostsFile.txt");
            hosts.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public void GetsName()
        {
            var name = HostInterface.GetHosts("TestHostsFile.txt").First().Hostname;
            name.Should().Be("upverter.com");
        }

        [Fact]
        public void GetsId()
        {
            var name = HostInterface.GetHosts("TestHostsFile.txt").First().Identifier;
            name.Should().Be("Upverter Development Server");
        }

        [Fact]
        public void GetsAddress()
        {
            var name = HostInterface.GetHosts("TestHostsFile.txt").First().Address;
            name.Should().Be("54.215.104.162");
        }

        [Fact]
        public void GetsEnabled()
        {
            var name = HostInterface.GetHosts("TestHostsFile.txt").First().Enabled;
            name.Should().Be(false);
        }

        [Fact]
        public void WritesFile()
        {
            var file = Path.GetTempFileName();
            HostInterface.WriteHosts(file, new List<Host>());
            File.Exists(file).Should().BeTrue();

            File.Delete(file);
        }

        
        public void WritesFileCorrectly(bool enabled, string ident, string hostname, string address)
        {
            var file = Path.GetTempFileName();
            var testHost = new Host(enabled, ident, hostname, address);
            HostInterface.WriteHosts(file, new []{testHost});

            var readHosts = HostInterface.GetHosts(file);
            readHosts.Count().Should().Be(1);

            var readHost = readHosts.First();
            readHost.Enabled.Should().Be(enabled);
            readHost.Identifier.Should().Be(ident);
            readHost.Hostname.Should().Be(hostname);
            readHost.Address.Should().Be(address);

            File.Delete(file);
        }

        [Fact]
        public void WritesHostsCorrectly_ParameterizedTests() // TODO: Convert to Theory
        {
            WritesFileCorrectly(true, "Ident!", "hostname.com", "192.143.144.200");
            WritesFileCorrectly(false, "Ident!", "hostname.com", "192.143.144.200");
            WritesFileCorrectly(true, "Ident! lkasdjflkawerfha ##", "hostname.com", "192.143.144.200");
        }
    }
}
