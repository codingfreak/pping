namespace codingfreaks.pping.Ui.ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using cfUtils.Logic.Core.Extensions;
    using cfUtils.Logic.Core.Utilities;

    using McMaster.Extensions.CommandLineUtils;

    [Command(Name = "pping", Description = "A port availability checker.")]
    public class PingLogic
    {
        #region methods

        /// <summary>
        /// Gets called by the logic of the <see cref="CommandAttribute" />.
        /// </summary>
        private async Task<int> OnExecuteAsync()
        {
            if (UseIpV6)
            {
                UseIpV4 = false;
            }
            if (Endless)
            {
                Repeats = int.MaxValue;
            }
            Console.WriteLine($"Starting pinging host {Host} on {ResolvedProtocol} port(s) {PortRange} {(Endless ? "infinite" : Repeats.ToString())} times:");
            var reachablePorts = 0;
            var closedPorts = 0;
            var hostIp = "-";
            if (ResvoleAddress)
            {
                // we have to perform address resolution    
                var entry = await Dns.GetHostEntryAsync(Host);
                if (entry.AddressList.Any())
                {
                    var target = UseIpV4 ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6;
                    hostIp = entry.AddressList.FirstOrDefault(he => he.AddressFamily == target)?.ToString() ?? "?";
                }
            }
            // start the process
            var currentPack = 0;
            for (var i = 0; i < Repeats; ++i)
            {
                var portOpen = true;
                Ports.ToList().ForEach(
                    port =>
                    {
                        try
                        {
                            portOpen &= NetworkUtil.IsPortOpened(Host, port, Timeout, UseUdp);
                            reachablePorts += portOpen ? 1 : 0;
                            closedPorts += portOpen ? 0 : 1;
                            var printResult = portOpen ? "OPEN" : "CLOSED";
                            if (Detailed && !portOpen)
                            {
                                printResult += $" ({NetworkUtil.LastCheckResult.ToString()})";
                            }
                            Console.Write("#{0,4} -> Pinging host {1} (IP:{2}) on {5} port {3} with timeout {4}: ", ++currentPack, Host, hostIp, port, Timeout, ResolvedProtocol);
                            Console.ForegroundColor = portOpen ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                            Console.WriteLine(printResult);
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"#{++currentPack,4} Error pinging host {Host}: {ex.Message}");
                            Console.ResetColor();
                        }
                    });
                if (!AutoStop || !portOpen)
                {
                    if (WaitTime > 0)
                    {
                        await Task.Delay(WaitTime);
                    }
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Finished pinging host {0} (IP:{1}). {2} pings sent ({3} OPEN, {4} CLOSED)", Host, hostIp, currentPack, reachablePorts, closedPorts);
            if (ReportFailedOnExit)
            {
                // return error level 1 if all pings where closed and 0 if any of them was open
                return reachablePorts > 0 ? 0 : 1;
            }
            if (ReportSucceededOnExit)
            {
                // return the amount of opened pings as the error level
                return reachablePorts;
            }
            return 0;
        }

        #endregion

        #region properties

        [Option("-a|--autostop", CommandOptionType.NoValue, Description = "If set, the app will stop working when it gets one OPEN-result.")]
        public bool AutoStop { get; set; }

        [Option("-4|--ipv4", CommandOptionType.NoValue, Description = "If set, the app will use IPv4 for resolutions.")]
        public bool UseIpV4 { get; set; } = true;

        [Option("-6|--ipv6", CommandOptionType.NoValue, Description = "If set, the app will use IPv6 for resolutions.")]
        public bool UseIpV6 { get; set; }

        [Option("-d|--detailed", CommandOptionType.NoValue, Description = "If set, the app will output some more detailled states.")]
        public bool Detailed { get; set; }

        [Option("-t|--endless", CommandOptionType.NoValue, Description = "If set, the app will run infinitely. (see -as option).")]
        public bool Endless { get; set; }

        [Required]
        [Argument(0, Name = "Host", Description = "The IP address or DNS name of the host to scan.")]
        public string Host { get; set; }

        [Required]
        [Argument(1, Name = "Port(s)", Description = "The port(s) to scan. Use '-' to specify a range of ports or list ports by separating them with ','.")]
        public string PortRange { get; set; }

        [Option("-r|--repeats", CommandOptionType.SingleValue, Description = "Defines the amount of requests which will be sent to the target (default is 4).")]
        public int Repeats { get; set; } = 4;

        [Option("-elf|--elfail", CommandOptionType.NoValue, Description = "If set, the app will return error level 0 on any open ping and error level 1 if all pings resulted in closed port.")]
        public bool ReportFailedOnExit { get; set; }

        [Option("-els|--elsucc", CommandOptionType.NoValue, Description = "If set, the app will return error level 0 on any open ping and error level 1 if all pings resulted in closed port.")]
        public bool ReportSucceededOnExit { get; set; }

        [Option("-res|--resolve", CommandOptionType.NoValue, Description = "If set, the app will resolve the DNS name of the target.")]
        public bool ResvoleAddress { get; set; }

        [Option("-tim|--timout", CommandOptionType.SingleValue, Description = "Defines the timeout in seconds the app will wait for requests to return.")]
        public int Timeout { get; set; } = 1;

        [Option("-u|--udp", CommandOptionType.NoValue, Description = "If set, a UDP ping will be performed.")]
        public bool UseUdp { get; set; }

        [Option("-w|--wait", CommandOptionType.SingleValue, Description = "Defines a time in milliseconds to wait before the first call is made. Defaults to 500.")]
        public int WaitTime { get; set; } = 500;

        /// <summary>
        /// Scans the <see cref="PortRange" /> property and interpretes it's value as a single number, a number range or a
        /// collection of numbers.
        /// </summary>
        private IEnumerable<int> Ports
        {
            get
            {
                if (PortRange.IsNullOrEmpty())
                {
                    return null;
                }
                var ports = new List<int>();
                if (PortRange.Contains(","))
                {
                    PortRange.Split(',').ToList().ForEach(p => ports.Add(int.Parse(p.Trim())));
                }
                else
                {
                    if (PortRange.Contains('-'))
                    {
                        var portsRanged = PortRange.Split('-');
                        var from = int.Parse(portsRanged[0]);
                        var to = int.Parse(portsRanged[1]);
                        for (var p = from; p <= to; p++)
                        {
                            ports.Add(p);
                        }
                    }
                    else
                    {
                        ports.Add(int.Parse(PortRange));
                    }
                }
                return ports.ToArray();
            }
        }

        /// <summary>
        /// Returns 'UDP' or 'TCP' depending on the setting in <see cref="UseUdp"/>.
        /// </summary>
        private string ResolvedProtocol => UseUdp ? "UDP" : "TCP";

        #endregion
    }
}