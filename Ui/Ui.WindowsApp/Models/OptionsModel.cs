namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Defines the model to store and retrieve all options from the data sink.
    /// </summary>
    public class OptionsModel : BaseModel
    {
        #region properties

        /// <summary>
        /// The list of job definitions.
        /// </summary>
        public IEnumerable<JobModel> JobDefinitions { get; set; }

        public WindowState WindowState { get; set; }

        public double WindowWidth { get; set; }

        public double WindowHeight { get; set; }

        public double WindowLeft { get; set; }

        public double WindowTop { get; set; }

        #endregion
    }
}