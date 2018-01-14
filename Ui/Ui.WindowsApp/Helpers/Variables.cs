namespace codingfreaks.pping.Ui.WindowsApp.Helpers
{
    using System;
    using System.Linq;

    using Autofac;

    /// <summary>
    /// Provides assembly-wide access to shared variables.
    /// </summary>
    public static class Variables
    {
        #region properties

        /// <summary>
        /// The ready-to-use AutoFac container build during app-ctor.
        /// </summary>
        public static IContainer AutoFacContainer { get; set; }

        #endregion
    }
}