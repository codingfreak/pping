namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Linq;

    public class PortModel
    {
        #region properties

        public int Port { get; set; }

        public string PortType => Tcp ? "TCP" : "UDP";

        public string Purpose { get; set; }

        public bool Tcp { get; set; }

        public bool Udp { get; set; }

        #endregion
    }
}