namespace codingfreaks.pping.Ui.WindowsApp.Messages
{
    using System;
    using System.Linq;

    using Models;

    public class PortAddInvokedMessage
    {
        #region constructors and destructors

        public PortAddInvokedMessage(PortModel portModel)
        {
            PortModel = portModel;
        }

        #endregion

        #region properties

        public PortModel PortModel { get; }

        #endregion
    }
}