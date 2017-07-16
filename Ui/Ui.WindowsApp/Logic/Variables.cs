namespace codingfreaks.pping.Ui.WindowsApp.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Models;

    public static class Variables
    {
        #region constants

        private static readonly Lazy<IEnumerable<PortModel>> _portFactory = new Lazy<IEnumerable<PortModel>>(
            () =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ports.txt");
                if (!File.Exists(path))
                {
                    return null;
                }                
                return File.ReadAllLines(path).Select(l => l.Split(';')).Select(
                    splitted => new PortModel
                    {
                        Port = int.Parse(splitted[1]),
                        Purpose = splitted[2],
                        Tcp = splitted[0] == "TCP",
                        Udp = splitted[0] == "UDP"
                    });
            });

        #endregion

        #region properties

        public static AddPortWindow AddPortWindow { get; set; }

        public static IEnumerable<PortModel> KnownPorts => _portFactory.Value;

        public static JobModel CurrentSelectedJob { get; set; }

        #endregion
    }
}